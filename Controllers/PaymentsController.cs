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
            try
            {
                var result = _vnpay.GetPaymentResult(Request.Query);
                int bookingId = ExtractBookingIdFromOrderInfo(result.Description);

                var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
                if (booking == null)
                {
                    return Ok(new { Message = "Không tìm thấy đơn đặt", VNPayResult = result });
                }

                booking.Status = BookingStatus.Completed;
                await _bookingRepository.UpdateBookingAsync(booking.Id, booking);

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

                return Ok(new { Message = "Xử lý callback thành công", BookingId = bookingId, VNPayResult = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý callback VNPay");
                return Ok(new { Message = "Lỗi khi xử lý callback VNPay", Error = ex.Message });
            }
        }

        //[HttpGet("vnpay-callback")]
        //[AllowAnonymous]
        //public async Task<IActionResult> VNPayCallback()
        //{
        //    try
        //    {
        //        var result = _vnpay.GetPaymentResult(Request.Query);
        //        if (!result.IsSuccess)
        //        {
        //            _logger.LogWarning("Thanh toán thất bại: {Description}", result.TransactionStatus.Description);

        //            var failedUrl = $"{_configuration["ClientUrl"]}/payment/failed" +
        //                            $"?message={Uri.EscapeDataString(result.TransactionStatus.Description)}";

        //            return RedirectPermanent(failedUrl);
        //        }

        //        int bookingId = ExtractBookingIdFromOrderInfo(result.PaymentResponse.Description);

        //        var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
        //        if (booking == null)
        //        {
        //            var notFoundUrl = $"{_configuration["ClientUrl"]}/payment/failed" +
        //                              $"?message={Uri.EscapeDataString("Không tìm thấy đơn đặt phòng")}";

        //            return RedirectPermanent(notFoundUrl);
        //        }

        //        booking.Status = BookingStatus.Completed;
        //        await _bookingRepository.UpdateBookingAsync(booking.Id, booking);

        //        var payment = new Payment
        //        {
        //            BookingId = bookingId,
        //            Amount = booking.TotalPrice,
        //            PaymentDate = DateTime.Now,
        //            PaymentMethod = "VNPay",
        //            Status = "Success",
        //            TransactionId = result.VnpayTransactionId.ToString(),
        //        };

        //        _context.Payments.Add(payment);
        //        await _context.SaveChangesAsync();

        //        var successUrl = $"{_configuration["ClientUrl"]}/payment/success" +
        //                         $"?bookingId={bookingId}&amount={booking.TotalPrice}";

        //        return RedirectPermanent(successUrl);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Lỗi khi xử lý callback VNPay");

        //        var errorUrl = $"{_configuration["ClientUrl"]}/payment/error" +
        //                       $"?message={Uri.EscapeDataString("Lỗi hệ thống khi xử lý thanh toán")}";

        //        return RedirectPermanent(errorUrl);
        //    }
        //}




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

        private int ExtractBookingIdFromOrderInfo(string description)
        {
            // Ví dụ: "Thanh toán dịch vụ nail #123"
            var parts = description.Split('#');
            if (parts.Length == 2 && int.TryParse(parts[1], out int id))
            {
                return id;
            }
            return 0;
        }

    }
}
