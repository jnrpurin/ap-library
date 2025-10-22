using System.Text.Json;
using LibraryManagementApp.Controllers;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.ControllerTests
{
    public class LoginControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ILogger<LoginController>> _logger;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _userServiceMock = new Mock<IUserService>();
            _configMock = new Mock<IConfiguration>();
            _logger = new Mock<ILogger<LoginController>>();
            _controller = new LoginController(_authServiceMock.Object, _userServiceMock.Object, _configMock.Object, _logger.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenModelStateInvalid()
        {
            _controller.ModelState.AddModelError("Username", "Required");

            var result = await _controller.Login(new LoginRequestDTO(){ Username = "abc", Password = "123" });

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request", badRequest.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserNotFound()
        {
            var dto = new LoginRequestDTO { Username = "adm", Password = "123" };
            _userServiceMock.Setup(s => s.GetUserByUsernameAsync("adm")).ReturnsAsync((User?)null);

            var result = await _controller.Login(dto);

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password", unauthorized.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenPasswordInvalid()
        {
            var dto = new LoginRequestDTO { Username = "adm", Password = "wrong" };
            var user = new User { Id = Guid.NewGuid(), Username = "adm", Email = "adm@adm.com", PasswordHash = "hashedPassword" };

            _userServiceMock.Setup(s => s.GetUserByUsernameAsync("adm")).ReturnsAsync(user);
            _authServiceMock.Setup(a => a.VerifyLoginAsync("adm", "wrong")).ReturnsAsync(false);

            var result = await _controller.Login(dto);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_ShouldReturnInternalServerError_WhenJwtConfigMissing()
        {
            var dto = new LoginRequestDTO { Username = "adm", Password = "123" };
            var user = new User { Id = Guid.NewGuid(), Username = "adm", Email = "adm@adm.com", PasswordHash = "hashedPassword" };

            _userServiceMock.Setup(s => s.GetUserByUsernameAsync("adm")).ReturnsAsync(user);
            _authServiceMock.Setup(a => a.VerifyLoginAsync("adm", "123")).ReturnsAsync(true);
            _configMock.Setup(c => c["Jwt:Key"]).Returns(string.Empty);

            var result = await _controller.Login(dto);

            var status = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, status.StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenValid()
        {
            var dto = new LoginRequestDTO { Username = "adm", Password = "123" };
            var user = new User { Id = Guid.NewGuid(), Username = "adm", Email = "adm@adm.com", PasswordHash = "hashedPassword" };

            _userServiceMock.Setup(s => s.GetUserByUsernameAsync("adm")).ReturnsAsync(user);
            _authServiceMock.Setup(a => a.VerifyLoginAsync("adm", "123")).ReturnsAsync(true);
            _configMock.Setup(c => c["Jwt:Key"]).Returns("key");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("audience");
            _authServiceMock.Setup(a => a.GenerateJwtToken(user, "key", "issuer", "audience")).Returns("mockToken");

            var result = await _controller.Login(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var root = JsonSerializer.SerializeToElement(okResult.Value!);

            Assert.Equal("Login successful", root.GetProperty("message").GetString());
            Assert.StartsWith("Bearer", root.GetProperty("token").GetString());
        }
    }
}
