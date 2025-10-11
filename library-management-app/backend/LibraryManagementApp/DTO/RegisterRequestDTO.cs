using System.ComponentModel;
using LibraryManagementApp.Enums;

namespace LibraryManagementApp.DTO
{
    public class RegisterRequestDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required UserRole Role { get; set; }
        public string? FullName { get; set; }
    }
}