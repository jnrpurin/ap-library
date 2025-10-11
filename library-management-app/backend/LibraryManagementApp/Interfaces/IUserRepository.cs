using LibraryManagementApp.Models;

namespace LibraryManagementApp.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task CreateAsync(User user);
    }
}