using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByUsernameAsync(string Username);
        Task<User> CreateUserAsync(RegisterRequestDTO dto);
        Task<User?> UpdateUserAsync(Guid id, UpdateUserDTO dto);
        Task<bool> DeactivateUserAsync(Guid id);
    }
}
