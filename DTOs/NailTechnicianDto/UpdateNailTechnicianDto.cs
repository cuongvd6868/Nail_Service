using Nail_Service.Models;
using System.ComponentModel.DataAnnotations;

namespace Nail_Service.DTOs.NailTechnicianDto
{
    public class UpdateNailTechnicianDto
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public string? FullName { get; set; }

        [MaxLength(500)]
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? Specialties { get; set; } // Các chuyên môn của thợ (ví dụ: "Nail Art", "Gel Nails", "Manicure/Pedicure")
        public int? NailSalonId { get; set; }

    }
}
