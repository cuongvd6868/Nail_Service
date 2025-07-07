using Microsoft.AspNetCore.Mvc;
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
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }
    }
}
