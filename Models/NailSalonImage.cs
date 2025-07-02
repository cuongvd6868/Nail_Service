using System.ComponentModel.DataAnnotations.Schema;

namespace Nail_Service.Models
{
    public class NailSalonImage
    {
        public int Id { get; set; }

        public int NailSalonId { get; set; }

        public string? Name { get; set; }

        public string? ImageUrl { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? ImageData { get; set; }

        public NailSalon? NailSalon { get; set; }
    }
}
