using System.ComponentModel.DataAnnotations;
using LibraryManagementApp.Enums;

namespace LibraryManagementApp.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public required string Username { get; set; }

        [Required, MaxLength(255)]
        public required string PasswordHash { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public required string Email { get; set; }

        public bool IsActive { get; set; } = true;

        [Required, MaxLength(50)]
        public UserRole Role { get; set; } = UserRole.User_Standard;

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; }
    }
}
