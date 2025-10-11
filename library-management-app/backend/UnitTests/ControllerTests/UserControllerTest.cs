using System.Text.Json;
using LibraryManagementApp.Controllers;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.ControllerTests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UsersController(_userServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithUsers()
        {
            var users = new List<User> { new() { Id = Guid.NewGuid(), Username = "adm", Email = "adm@adm.com", PasswordHash = "123" } };
            _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Single(returnUsers);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenUserExists()
        {
            var id = Guid.NewGuid();
            var user = new User { Id = id, Username = "test", Email = "adm@adm.com", PasswordHash = "123" };
            _userServiceMock.Setup(s => s.GetUserByIdAsync(id)).ReturnsAsync(user);

            var result = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal("test", returnUser.Username);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenUserNotExists()
        {
            var id = Guid.NewGuid();
            _userServiceMock.Setup(s => s.GetUserByIdAsync(id)).ReturnsAsync((User)null);

            var result = await _controller.GetById(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var dto = new RegisterRequestDTO { Username = "adm", Password = "123", Email = "adm@adm.com", Role = LibraryManagementApp.Enums.UserRole.Admin };
            var user = new User { Id = Guid.NewGuid(), Username = "adm", Email = "adm@adm.com", PasswordHash = "123" };

            _userServiceMock.Setup(s => s.CreateUserAsync(dto)).ReturnsAsync(user);

            var result = await _controller.Create(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), created.ActionName);
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenUserExists()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateUserDTO { Email = "mail@test.com" };
            var user = new User { Id = id, Username = "adm", Email = "adm@adm.com", PasswordHash = "123" };
            _userServiceMock.Setup(s => s.UpdateUserAsync(id, dto)).ReturnsAsync(user);

            var result = await _controller.Update(id, dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user, ok.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenUserNotExists()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateUserDTO();
            _userServiceMock.Setup(s => s.UpdateUserAsync(id, dto)).ReturnsAsync((User)null);

            var result = await _controller.Update(id, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Deactivate_ShouldReturnOk_WhenUserDeactivated()
        {
            var id = Guid.NewGuid();
            _userServiceMock.Setup(s => s.DeactivateUserAsync(id)).ReturnsAsync(true);

            var result = await _controller.Deactivate(id);

            var ok = Assert.IsType<OkObjectResult>(result);
            var json = JsonSerializer.Serialize(ok.Value);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Assert.Equal("User deactivated successfully", root.GetProperty("message").GetString());
        }

        [Fact]
        public async Task Deactivate_ShouldReturnNotFound_WhenUserNotExists()
        {
            var id = Guid.NewGuid();
            _userServiceMock.Setup(s => s.DeactivateUserAsync(id)).ReturnsAsync(false);

            var result = await _controller.Deactivate(id);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
