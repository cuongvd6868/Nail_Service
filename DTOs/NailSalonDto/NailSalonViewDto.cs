using Nail_Service.Models;

namespace Nail_Service.DTOs.NailSalonDto
{
    public class NailSalonViewDto
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
        //public List<Amenity>? Amenities { get; set; } = new List<Amenity>();
        public List<NailServiceD>? NailServices { get; set; } = new List<NailServiceD>();
        public List<NailTechnician>? NailTechnicians { get; set; } = new List<NailTechnician>();
    }
}
