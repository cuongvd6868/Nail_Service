using System.ComponentModel.DataAnnotations;

namespace Nail_Service.Models
{
    public class Promotion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã khuyến mãi là bắt buộc")]
        [StringLength(50, ErrorMessage = "Mã khuyến mãi không được vượt quá 50 ký tự")]
        public string PromotionCode { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Số tiền giảm giá là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền giảm giá phải là số không âm")]
        public decimal DiscountAmount { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Trạng thái hoạt động là bắt buộc")]
        public bool IsActive { get; set; }

        public List<BookingNail>? BookingNails { get; set; } = new List<BookingNail>();
    }
}
