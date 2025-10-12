using LibraryManagementApp.Helper;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagementApp.Services
{
    public class AuthService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<bool> VerifyLoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                return false;

            return PasswordHelper.VerifyPassword(password, user.PasswordHash);
        }

        public string GenerateJwtToken(User user, string secretKey, string issuer, string audience)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Guid GetAuthenticatedUserId()
        {
            var userIdClaimPrincipal = (_httpContextAccessor.HttpContext?.User) ?? throw new UnauthorizedAccessException("User ID not found.");

            var userIdClaim = userIdClaimPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                throw new UnauthorizedAccessException("User ID (Claim) not found or invalid.");

            return userId;
        }
    }
}
