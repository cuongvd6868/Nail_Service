using Microsoft.AspNetCore.Identity;

namespace Nail_Service.Models
{
    public class AppUser : IdentityUser
    {
        public NailTechnician? NailTechnicianProfile { get; set; }
        public List<BookingNail>? Bookings { get; set; } = new List<BookingNail>();
        public List<Reviews>? Reviews { get; set; } = new List<Reviews>();
        public List<NailSalonFavorite>? NailSalonFavorites { get; set; } = new List<NailSalonFavorite>();
        public List<Post>? Posts { get; set; } = new List<Post>();
    }
}
