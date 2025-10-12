using LibraryManagementApp.Models;

namespace LibraryManagementApp.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}