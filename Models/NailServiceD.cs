using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Nail_Service.Models
{
    public class NailServiceD
    {
        public int Id { get; set; } 
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } 
        [MaxLength(500)]
        public string Description { get; set; } 
        [Required]
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal Price { get; set; } 
        public int Duration { get; set; } // Thời gian thực hiện dịch vụ tính bằng phút
        public string ImageUrl { get; set; } 
        public bool IsActive { get; set; } = true; 
        public int? NailServiceDCategoryId { get; set; } 

        public NailServiceDCategory? NailServiceDCategory { get; set; } 
        public List<NailSalon>? NailSalons { get; set; } 
        public List<BookingNail>? Bookings { get; set; } = new List<BookingNail>();

    }
}
