using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Nail_Service.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string ContentMarkdown { get; set; } // Lưu nội dung gốc Markdown

        [Column(TypeName = "text")]
        public string ContentHtml { get; set; } // Lưu HTML đã render
        [Column(TypeName = "nvarchar(max)")]
        public string? ImageData { get; set; }
        public string Status { get; set; } = "Draft"; //Draft, Published, Archived 
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public AppUser? Author { get; set; }
    }
}
