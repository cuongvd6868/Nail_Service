using System.ComponentModel.DataAnnotations;

namespace Nail_Service.Models
{
    public class NailServiceDCategory
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } // Ví dụ: "Manicure", "Pedicure", "Nail Art", "Đắp Móng"
        public string Description { get; set; }
        public List<NailServiceD>? NailServiceDs { get; set; }
    }
}
