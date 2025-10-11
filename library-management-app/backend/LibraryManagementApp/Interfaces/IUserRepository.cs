using LibraryManagementApp.Models;

namespace LibraryManagementApp.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);

        Task CreateAsync(User user);
        void Update(User user);
        Task SaveChangesAsync();
    }
}