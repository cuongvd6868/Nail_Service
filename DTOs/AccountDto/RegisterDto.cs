using System.ComponentModel.DataAnnotations;

namespace Nail_Service.DTOs.AccountDto
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string? Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;

    }
}
