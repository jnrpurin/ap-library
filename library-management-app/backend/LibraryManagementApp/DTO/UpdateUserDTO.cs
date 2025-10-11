using LibraryManagementApp.Enums;

namespace LibraryManagementApp.DTO
{
    public class UpdateUserDTO
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
