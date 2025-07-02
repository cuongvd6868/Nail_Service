namespace Nail_Service.Models
{
    public class NailSalon
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Address { get; set; } 
        public string PhoneNumber { get; set; } 
        public string Email { get; set; } 
        public string Description { get; set; } 

        public TimeSpan OpeningTime { get; set; } 
        public TimeSpan ClosingTime { get; set; } 
        public string DaysOpen { get; set; } 
        public string ImageUrl { get; set; } 
        public double AverageRating { get; set; } 
        public int NumberOfReviews { get; set; } 
        public int LocationId { get; set; }

        // navigation properties
        public List<NailSalonImage> NailSalonImages { get; set; } = new List<NailSalonImage>();
        public List<Amenity>? Amenities { get; set; } = new List<Amenity>(); 
        public Location? Location { get; set; }
        public List<NailServiceD>? NailServices { get; set; } = new List<NailServiceD>();
        public List<NailTechnician>? NailTechnicians { get; set; } = new List<NailTechnician>();
        public List<BookingNail>? Bookings { get; set; } = new List<BookingNail>();
        public List<Reviews>? Reviews { get; set; } = new List<Reviews>();
        public AppUser AppUser { get; set; }

    }
}