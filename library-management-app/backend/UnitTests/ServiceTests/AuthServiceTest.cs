using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LibraryManagementApp.Helper;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using LibraryManagementApp.Services;
using LibraryManagementApp.Enums;
using Moq;

namespace UnitTests.ServiceTests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authService = new AuthService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task VerifyLoginAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("unknown"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _authService.VerifyLoginAsync("unknown", "123");

            // Assert
            Assert.False(result);
            _userRepositoryMock.Verify(r => r.GetByUsernameAsync("unknown"), Times.Once);
        }

        [Fact]
        public async Task VerifyLoginAsync_ShouldReturnTrue_WhenPasswordMatches()
        {
            // Arrange
            var user = new User
            {
                Username = "adm",
                PasswordHash = PasswordHelper.HashPassword("123"),
                Email = "email@email.com",
                Role = UserRole.Admin
            };

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("adm"))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.VerifyLoginAsync("adm", "123");

            // Assert
            Assert.True(result);
            _userRepositoryMock.Verify(r => r.GetByUsernameAsync("adm"), Times.Once);
        }

        [Fact]
        public async Task VerifyLoginAsync_ShouldReturnFalse_WhenPasswordDoesNotMatch()
        {
            // Arrange
            var user = new User
            {
                Username = "adm",
                PasswordHash = PasswordHelper.HashPassword("123"),
                Email = "email@email.com",
                Role = UserRole.Admin
            };

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("adm"))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.VerifyLoginAsync("adm", "wrongpassword");

            // Assert
            Assert.False(result);
            _userRepositoryMock.Verify(r => r.GetByUsernameAsync("adm"), Times.Once);
        }

        [Fact]
        public void GenerateJwtToken_ShouldGenerateValidToken()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "adm",
                Email = "email@email.com",
                Role = UserRole.Admin,
                PasswordHash = string.Empty 
            };

            var secretKey = "MySuperSecretKey12345!-6789034563623562345672356"; // Should be at least 16 characters for HMACSHA256
            var issuer = "LibraryManagementApp";
            var audience = "LibraryManagementAppUsers";

            // Act
            var tokenString = _authService.GenerateJwtToken(user, secretKey, issuer, audience);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(tokenString));

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            // Validate claims
            Assert.Equal("adm", token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value);
            Assert.Equal(user.Id.ToString(), token.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
            Assert.Equal("Admin", token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);

            // Validate issuer and audience
            Assert.Equal(issuer, token.Issuer);
            Assert.Contains(audience, token.Audiences);

            // Validate expiration (~1h)
            Assert.True(token.ValidTo > DateTime.UtcNow);
        }

        [Fact]
        public void GenerateJwtToken_ShouldThrow_WhenSecretKeyInvalid()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "test", Email = "email@email.com", Role = UserRole.Admin, PasswordHash = string.Empty };
            var invalidKey = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _authService.GenerateJwtToken(user, invalidKey, "issuer", "audience"));
        }
    }
}
