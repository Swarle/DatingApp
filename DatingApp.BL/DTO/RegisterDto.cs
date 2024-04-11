
using System.ComponentModel.DataAnnotations;

namespace DatingApp.BL.DTO
{
    public class RegisterDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        [StringLength(maximumLength: 8, MinimumLength = 4)]
        public required string Password { get; set; }
        [Required]
        public required string KnownAs { get; set; }
        [Required]
        public DateOnly? DateOfBirth { get; set; }
        [Required]
        public required string Gender { get; set; }
        [Required]
        public required string City { get; set; }
        [Required]
        public required string Country { get; set; }
        
    }
}
