using Nail_Service.Data;
using Nail_Service.Models;
using Microsoft.EntityFrameworkCore;
using Nail_Service.DTOs.BookingDto;

namespace Nail_Service.Repository.Impl
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        private readonly INailServiceDRepository _nailServiceDRepository;

        public BookingRepository(AppDbContext context, INailServiceDRepository nailServiceDRepository)
        {
            _context = context;
            _nailServiceDRepository = nailServiceDRepository;
        }

        public async Task CreateBookingAsync(string userId, CreateBookingDto bookingDto)
        {
            var selectedServices = await _nailServiceDRepository.GetServicesBYIdAsync(bookingDto.SelectedServiceIds);
            if (selectedServices == null || !selectedServices.Any())
            {
                throw new ArgumentException("No valid services selected for the booking.");
            }
            decimal totalPrice = selectedServices.Sum(s => s.Price); // total booking price = total nailservices price
            TimeSpan timeSpan = TimeSpan.FromMinutes(selectedServices.Sum(s => s.Duration)); // total booking duration = total nailservices duration
            var booking = new BookingNail
            {
                BookingDateTime = bookingDto.BookingDateTime,
                Duration = timeSpan,
                TotalPrice = totalPrice,
                Notes = bookingDto.Notes,
                Status = BookingStatus.Pending,
                CreatedDate = DateTime.UtcNow,
                CustomerId = userId,
                NailTechnicianId = bookingDto.NailTechnicianId,
                NailSalonId = bookingDto.NailSalonId,
                //NailServices = selectedServices.ToList()
            };

            foreach (var service in selectedServices)
            {
                booking.NailServices.Add(service);
            }
            _context.Bookings.Add(booking);
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
                .Include(b => b.NailServices)
                .Include(b => b.NailSalon)
                .Include(b => b.Customer)
                .ToListAsync();
        }

        public async Task<BookingNail> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.NailServices)
                .Include(b => b.NailSalon)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<BookingNail>> GetBookingsByUserIdAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.NailServices)
                .Include(b => b.NailSalon)
                .Where(b => b.CustomerId == userId)
                .ToListAsync();
        }

        public async Task UpdateBookingAsync(int id, BookingNail booking)
        {
            var bookingToUpdate = await _context.Bookings.FindAsync(id);
            if (bookingToUpdate == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }
            bookingToUpdate.BookingDateTime = booking.BookingDateTime;
            bookingToUpdate.Duration = booking.Duration;
            bookingToUpdate.TotalPrice = booking.TotalPrice;
            bookingToUpdate.Notes = booking.Notes;
            bookingToUpdate.Status = booking.Status;
            bookingToUpdate.NailTechnicianId = booking.NailTechnicianId;
            bookingToUpdate.NailSalonId = booking.NailSalonId;
            // Clear existing services and add new ones
            //bookingToUpdate.NailServices.Clear();
            //foreach (var service in booking.NailServices)
            //{
            //    bookingToUpdate.NailServices.Add(service);
            //}
            _context.Bookings.Update(bookingToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingStatusAsync(int id, BookingStatusUpdateDto bookingStatusDto)
        {
            var bookingToUpdate = await _context.Bookings.FindAsync(id);
            if (bookingToUpdate == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }
            bookingToUpdate.Status = bookingStatusDto.Status;

            _context.Bookings.Update(bookingToUpdate);
            await _context.SaveChangesAsync();
        }
    }
}
