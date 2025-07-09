using Nail_Service.DTOs.BookingDto;
using Nail_Service.Models;

namespace Nail_Service.Repository
{
    public interface IBookingRepository
    {
        Task<IEnumerable<BookingNail>> GetAllBookingsAsync();
        Task<BookingNail> GetBookingByIdAsync(int id);
        Task CreateBookingAsync(string userId, CreateBookingDto bookingDto);
        Task UpdateBookingAsync(int id, BookingNail booking);
        Task UpdateBookingStatusAsync(int id, BookingStatusUpdateDto bookingStatusDto);
        Task DeleteBookingAsync(int id);
        Task<IEnumerable<BookingNail>> GetBookingsByUserIdAsync(string userId);
    }
}
