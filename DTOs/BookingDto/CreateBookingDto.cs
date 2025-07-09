namespace Nail_Service.DTOs.BookingDto
{
    public class CreateBookingDto
    {
        public int? Id { get; set; }
        public DateTime BookingDateTime { get; set; }
        public string? Notes { get; set; }
        public string? CustomerId { get; set; }
        public int NailTechnicianId { get; set; }
        public int NailSalonId { get; set; }
        public List<int> SelectedServiceIds { get; set; } = new List<int>();
    }
}
