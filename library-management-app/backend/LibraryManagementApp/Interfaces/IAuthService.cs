using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Interfaces
{
    public interface IAuthService
    {
        Task<bool> VerifyLoginAsync(string username, string password);
        string GenerateJwtToken(User user, string secretKey, string issuer, string audience);
    }
}
