using Microsoft.EntityFrameworkCore;
using Nail_Service.Models;
using System.ComponentModel.DataAnnotations;

namespace Nail_Service.Models
{
    public class Reviews
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID là bắt buộc")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "NailSalon ID là bắt buộc")]
        public int NailSalonId { get; set; }
        [Required(ErrorMessage = "BookingNailId ID là bắt buộc")]
        public int BookingNailId { get; set; }
        [Required(ErrorMessage = "Điểm đánh giá là bắt buộc")]
        [Range(1, 5, ErrorMessage = "Điểm đánh giá phải từ 1 đến 5")]
        public int Rating { get; set; } // Rating given by the user (e.g., 1 to 5 stars)

        [StringLength(500, ErrorMessage = "Bình luận không được vượt quá 500 ký tự")]
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public AppUser? User { get; set; }
        public NailSalon? NailSalon { get; set; }
        public BookingNail? BookingNail { get; set; }   

    }
}

//// Cấu hình Unique Index cho BookingNailId trong bảng Reviews
//modelBuilder.Entity<Reviews>()
//    .HasIndex(r => r.BookingNailId)
//    .IsUnique(); // Đảm bảo chỉ có một đánh giá cho mỗi BookingNailId