namespace Nail_Service.Models
{
    public class Location
    {
        public int Id { get; set; } 
        public string District { get; set; } 
        public string City { get; set; }
        public string Country { get; set; } 
        public List<NailSalon>? NailSalons { get; set; } = new List<NailSalon>();
    }
}
