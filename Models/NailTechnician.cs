using System.ComponentModel.DataAnnotations;

namespace Nail_Service.Models
{
    public class NailTechnician
    {
        public int Id { get; set; } 

        [Required]
        public string AppUserId { get; set; } 

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; } 
        public string ProfilePictureUrl { get; set; }
        public int YearsOfExperience { get; set; }
        public string Specialties { get; set; } // Các chuyên môn của thợ (ví dụ: "Nail Art", "Gel Nails", "Manicure/Pedicure")
        public double AverageRating { get; set; }
        public int NumberOfReviews { get; set; } 

        [Required]
        public int NailSalonId { get; set; }

        // Navigation properties
        public List<AvailabilitySlot>? AvailabilitySlots { get; set; } = new List<AvailabilitySlot>(); // Danh sách các khung giờ làm việc   
        public NailSalon? NailSalon { get; set; }
        public AppUser? AppUser { get; set; }
        public List<BookingNail>? BookingNails { get; set; } = new List<BookingNail>(); // Danh sách các booking liên quan đến thợ này
    }
}