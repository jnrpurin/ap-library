using System.ComponentModel;

namespace LibraryManagementApp.DTO
{
    public class LoginRequestDTO
    {
        public string Username { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}