using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;
using Nail_Service.Data;
using Nail_Service.Models;
using Nail_Service.Repository;
using System.Text.RegularExpressions;


namespace Nail_Service.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<PaymentsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IVnpay _vnpay;

        public PaymentsController(
            IBookingRepository bookingRepository,
            ILogger<PaymentsController> logger,
            IConfiguration configuration,
            AppDbContext context,
            IVnpay vnpay)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
            _configuration = configuration;
            _context = context;

            _vnpay = vnpay;
            _vnpay.Initialize(
                _configuration["Vnpay:TmnCode"],
                _configuration["Vnpay:HashSecret"],
                _configuration["Vnpay:BaseUrl"],
                _configuration["Vnpay:ReturnUrl"]);
        }

        [HttpGet("create-url/{bookingId}")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentUrl(int bookingId)
        {
            try
            {
                var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
                if (booking == null)
                {
                    return NotFound(new { Message = "Không tìm thấy đơn đặt" });
                }

                //if (!booking.Status.Equals(BookingStatus.Pending))
                //{
                //    return BadRequest(new { Message = "Đơn đặt không thể thanh toán" });
                //}

                var ipAddress = NetworkHelper.GetIpAddress(HttpContext);

                var request = new PaymentRequest
                {
                    PaymentId = DateTime.Now.Ticks,
                    Money = (double)booking.TotalPrice,
                    Description = $"Thanh toán dịch vụ nail #{bookingId}",
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY,
                    CreatedDate = DateTime.Now,
                    Currency = Currency.VND,
                    Language = DisplayLanguage.Vietnamese
                };

                var paymentUrl = _vnpay.GetPaymentUrl(request);

                // Lưu log hoặc tạo payment record nếu cần

                return Ok(new
                {
                    PaymentUrl = paymentUrl,
                    PaymentId = request.PaymentId,
                    BookingId = bookingId,
                    Amount = booking.TotalPrice
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo URL thanh toán VNPAY");
                return StatusCode(500, new { Message = "Lỗi hệ thống khi tạo thanh toán" });
            }
        }


        [HttpGet("vnpay-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> VNPayCallback()
        {
            var queryString = string.Join("&", Request.Query.Select(q => $"{q.Key}={q.Value}"));
            _logger.LogInformation("VNPAY Callback Received. Query Parameters: {Query}", queryString);

            try
            {
                var result = _vnpay.GetPaymentResult(Request.Query);

                _logger.LogInformation(
                    "VNPAY Result: IsSuccess={IsSuccess}, TransactionStatus={TransactionStatus}, VnpayTransactionId={VnpayTransactionId}",
                    result.IsSuccess,
                    result.TransactionStatus?.Description,
                    result.VnpayTransactionId
                );

                // ❌ Giao dịch không thành công
                if (!result.IsSuccess)
                {
                    string errorMessage = result.TransactionStatus?.Description ?? result.Description ?? "Giao dịch không thành công";
                    _logger.LogWarning("Thanh toán thất bại từ VNPAY: {ErrorMessage}", errorMessage);

                    var failedUrl = $"http://localhost:3000/payment/failed" +
                                    $"?message={Uri.EscapeDataString($"Giao dịch VNPAY thất bại: {errorMessage}")}";
                    return RedirectPermanent(failedUrl);
                }

                // ✅ Lấy OrderInfo từ query string
                string orderInfo = Request.Query["vnp_OrderInfo"];
                if (string.IsNullOrEmpty(orderInfo))
                {
                    _logger.LogError("VNPAY Callback: vnp_OrderInfo trống. Không thể trích xuất Booking ID.");
                    var errorUrl = $"http://localhost:3000/payment/failed" +
                                   $"?message={Uri.EscapeDataString("Lỗi xử lý: Không có thông tin đơn hàng từ VNPAY")}";
                    return RedirectPermanent(errorUrl);
                }

                // ✅ Trích xuất Booking ID bằng Regex
                int bookingId = ExtractBookingIdFromOrderInfo(orderInfo);
                if (bookingId == 0)
                {
                    _logger.LogError("VNPAY Callback: Không thể trích xuất Booking ID hợp lệ từ vnp_OrderInfo: {OrderInfo}", orderInfo);
                    var invalidIdUrl = $"http://localhost:3000/payment/failed" +
                                        $"?message={Uri.EscapeDataString("Lỗi xử lý: Booking ID trong thông tin đơn hàng không hợp lệ")}";
                    return RedirectPermanent(invalidIdUrl);
                }

                _logger.LogInformation("VNPAY Callback: Đã trích xuất Booking ID: {BookingId} từ vnp_OrderInfo: {OrderInfo}", bookingId, orderInfo);

                // 🔎 Tìm đơn đặt
                var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
                if (booking == null)
                {
                    _logger.LogWarning("VNPAY Callback: Không tìm thấy đơn đặt với ID: {BookingId}", bookingId);
                    var notFoundUrl = $"http://localhost:3000/payment/failed" +
                                        $"?message={Uri.EscapeDataString($"Không tìm thấy đơn đặt (ID: {bookingId}) trong hệ thống")}";
                    return RedirectPermanent(notFoundUrl);
                }

                // ✅ Đã thanh toán trước đó
                if (booking.Status == BookingStatus.Completed)
                {
                    _logger.LogInformation("Booking ID {BookingId} đã ở trạng thái Completed. Bỏ qua xử lý trùng lặp.", bookingId);
                    var alreadyCompletedUrl = $"http://localhost:3000/payment/success" +
                                              $"?bookingId={bookingId}&amount={booking.TotalPrice}&message={Uri.EscapeDataString("Đơn hàng đã được thanh toán trước đó")}";
                    return RedirectPermanent(alreadyCompletedUrl);
                }

                // ✅ Cập nhật trạng thái booking
                booking.Status = BookingStatus.Completed;
                await _bookingRepository.UpdateBookingAsync(booking.Id, booking);
                _logger.LogInformation("Đã cập nhật trạng thái Booking ID {BookingId} thành {Status}.", booking.Id, BookingStatus.Completed);

                // ✅ Lưu bản ghi thanh toán
                var payment = new Payment
                {
                    BookingId = bookingId,
                    Amount = booking.TotalPrice,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "VNPay",
                    Status = "Success",
                    TransactionId = result.VnpayTransactionId.ToString()
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Đã lưu bản ghi thanh toán cho Booking ID {BookingId}, Payment ID: {PaymentId}.", bookingId, payment.Id);

                // ✅ Chuyển hướng thành công
                var successUrl = $"http://localhost:3000/payment/success" +
                                 $"?bookingId={bookingId}&amount={booking.TotalPrice}";
                _logger.LogInformation("Chuyển hướng đến trang thành công: {SuccessUrl}", successUrl);
                return RedirectPermanent(successUrl);
            }
            catch (Exception ex)
            {
                var queryStringInCatch = string.Join("&", Request.Query.Select(q => $"{q.Key}={q.Value}"));
                _logger.LogError(ex, "Lỗi nghiêm trọng khi xử lý callback VNPay. Toàn bộ Query Parameters: {Query}", queryStringInCatch);

                var errorUrl = $"http://localhost:3000/payment/failed" +
                               $"?message={Uri.EscapeDataString("Lỗi hệ thống không xác định khi xử lý thanh toán. Vui lòng liên hệ hỗ trợ.")}";
                return RedirectPermanent(errorUrl);
            }
        }


        [HttpGet("{paymentId}/status")]
        [Authorize]
        public async Task<IActionResult> GetPaymentStatus(int paymentId)
        {
            try
            {
                var payment = await _context.Payments
                    .Include(p => p.Booking)
                    .FirstOrDefaultAsync(p => p.Id == paymentId);

                if (payment == null)
                {
                    return NotFound(new { Message = "Không tìm thấy thanh toán" });
                }

                return Ok(new
                {
                    payment.Id,
                    payment.BookingId,
                    payment.Amount,
                    payment.PaymentMethod,
                    payment.Status,
                    payment.TransactionId,
                    PaymentDate = payment.PaymentDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    BookingStatus = payment.Booking?.Status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy trạng thái thanh toán");
                return StatusCode(500, new { Message = "Lỗi hệ thống" });
            }
        }

        [HttpGet("booking/{bookingId}/payments")]
        [Authorize]
        public async Task<IActionResult> GetPaymentsByBooking(int bookingId)
        {
            try
            {
                var payments = await _context.Payments
                    .Where(p => p.BookingId == bookingId)
                    .Select(p => new
                    {
                        p.Id,
                        p.Amount,
                        p.PaymentDate,
                        p.PaymentMethod,
                        p.Status,
                        p.TransactionId
                    })
                    .ToListAsync();

                if (!payments.Any())
                {
                    return NotFound(new { Message = "Không tìm thấy thanh toán nào cho booking này" });
                }

                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy danh sách thanh toán cho booking {bookingId}");
                return StatusCode(500, new { Message = "Lỗi hệ thống" });
            }
        }

        private int ExtractBookingIdFromOrderInfo(string orderInfo)
        {
            // Ví dụ: "Thanh toán dịch vụ nail #22"
            var match = Regex.Match(orderInfo, @"#(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int bookingId))
            {
                return bookingId;
            }
            return 0;
        }


    }
}
