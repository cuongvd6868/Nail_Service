using Microsoft.AspNetCore.Mvc;
using Nail_Service.DTOs.BookingDto;
using Nail_Service.Extensions;
using Nail_Service.Repository;

namespace Nail_Service.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingNailController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingNailController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound(new { Message = $"Booking with ID {id} not found." });
            }
            return Ok(booking);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetBookingsByUserId()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            if (bookings == null || !bookings.Any())
            {
                return NotFound(new { Message = "No bookings found for the user." });
            }
            return Ok(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto bookingDto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
            if (bookingDto == null)
            {
                return BadRequest(new { Message = "Booking data is required." });
            }
            try
            {
                // Gọi phương thức repository và lưu ID được trả về
                var newBookingId = await _bookingRepository.CreateBookingAsync(userId, bookingDto);

                return Ok(new
                {
                    id = newBookingId, // Trả về ID mới được lấy từ repository
                    message = "Booking created successfully.",
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            // catch (Exception ex) // Thêm catch tổng quát hơn nếu cần để bắt các lỗi khác
            // {
            //     return StatusCode(500, new { Message = "An error occurred while creating the booking.", Error = ex.Message });
            // }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatusUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Booking status data is required." });
            }
            try
            {
                await _bookingRepository.UpdateBookingStatusAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
