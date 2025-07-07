using Nail_Service.Data;
using Nail_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Nail_Service.Repository.Impl
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateBookingAsync(string userId, BookingNail booking)
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking), "Booking cannot be null.");
            }
            booking.CustomerId = userId;
            booking.Status = BookingStatus.Pending;
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingNail>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.NailTechnician)
                .Include(b => b.NailSalon)
                .ToListAsync();
        }

        public async Task<BookingNail> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id)
                   ?? throw new KeyNotFoundException($"Booking with ID {id} not found.");
        }

        public async Task<IEnumerable<BookingNail>> GetBookingsByUserIdAsync(string userId)
        {
            return await _context.Bookings
                .Where(b => b.CustomerId == userId)
                .Include(b => b.NailTechnician)
                .Include(b => b.NailSalon)
                .ToListAsync();
        }

        public async Task UpdateBookingAsync(int id, BookingNail booking)
        {
            var existingBooking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBooking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }
            existingBooking.NailTechnicianId = booking.NailTechnicianId;
            existingBooking.NailSalonId = booking.NailSalonId;
            existingBooking.TotalPrice = booking.TotalPrice;
            existingBooking.Status = booking.Status;
            _context.Bookings.Update(existingBooking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingStatusAsync(int id, BookingNail booking)
        {
            var bk = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
            if (bk == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }
            bk.Status = booking.Status;
        }
    }
}
