using System.ComponentModel.DataAnnotations;

namespace DatingApp.BL.DTO.UserDTOs
{
    public class LoginDto
    {
        [Required]
        public required  string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
