using Nail_Service.Models;
using System.ComponentModel.DataAnnotations;

namespace Nail_Service.DTOs.NailTechnicianDto
{
    public class CreateNailTechnicianDto
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public string? FullName { get; set; }

        [MaxLength(500)]
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? Specialties { get; set; } // Các chuyên môn của thợ (ví dụ: "Nail Art", "Gel Nails", "Manicure/Pedicure")
        public string? Status { get; set; } = "Available"; // Trạng thái của thợ (ví dụ: "Available", "Busy", "On Leave")
        public bool? IsActive { get; set; } = true; // Trạng thái hoạt động của thợ (ví dụ: true, false)
        public int? NailSalonId { get; set; }
    }
}
