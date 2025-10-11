using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTO
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public required string Username { get; set; }
        
        [PasswordPropertyText]
        [Required(ErrorMessage = "Password is required.")]
        public required string Password { get; set; }
    }
}