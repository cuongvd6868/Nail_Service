namespace Nail_Service.Models
{
    public class AvailabilitySlot
    {
        public int Id { get; set; }

        public int NailTechnicianId { get; set; } 
        public DateTime StartTime { get; set; } // Thời gian bắt đầu của khung giờ
        public DateTime EndTime { get; set; }   // Thời gian kết thúc của khung giờ

        // Có thể thêm trường này để dễ dàng đánh dấu slot đã bị đặt
        // public bool IsBooked { get; set; } = false; 

        // Hoặc liên kết trực tiếp với BookingNail nếu slot đã được đặt
        public int? BookingNailId { get; set; } // Có thể null nếu chưa được đặt

        public NailTechnician NailTechnician { get; set; }
        public BookingNail? BookingNail { get; set; } 
    }
}