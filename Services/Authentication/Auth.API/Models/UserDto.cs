using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 50 characters.")]
        public string Password { get; set; } = null!;
    }
}