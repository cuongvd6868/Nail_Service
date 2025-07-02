namespace Nail_Service.Models
{
    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 
        public string? IconUrl { get; set; } 
        public List<NailSalon>? NailSalons { get; set; } 
    }
}