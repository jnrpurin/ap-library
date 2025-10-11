using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterRequestDTO request);
        Task<bool> VerifyLoginAsync(string username, string password);
        Task<User?> GetUserByUsernameAsync(string username);
        string GenerateJwtToken(User user, string secretKey, string issuer, string audience);
    }
}
