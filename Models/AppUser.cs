using Microsoft.AspNetCore.Identity;

namespace Nail_Service.Models
{
    public class AppUser : IdentityUser
    {
        public NailTechnician? NailTechnicianProfile { get; set; }
        public NailSalon? NailSalonProfile { get; set; }
        public List<BookingNail>? Bookings { get; set; } = new List<BookingNail>();
        public List<Reviews>? Reviews { get; set; } = new List<Reviews>();
        public List<NailSalonFavorite>? nailSalonFavorites { get; set; } = new List<NailSalonFavorite>();
        public List<Post>? Posts { get; set; } = new List<Post>();
    }
}
