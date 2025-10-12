using LibraryManagementApp.DTO;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using LibraryManagementApp.Services;
using LibraryManagementApp.Enums;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace UnitTests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new() { Id = Guid.NewGuid(), Username = "user1", PasswordHash = "123", Email = "email@mail.com", Role = UserRole.Standard  },
                new() { Id = Guid.NewGuid(), Username = "user2", PasswordHash = "1234", Email = "email2@mail.com", Role = UserRole.Admin  }
            };
            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<User>)result).Count);
            _userRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ShouldReturnUser_WhenExists()
        {
            // Arrange
            var expectedUser = new User { Id = Guid.NewGuid(), Username = "adm", PasswordHash = "123", Email = "email@mail.com", Role = UserRole.Standard  };
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("adm")).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByUsernameAsync("adm");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("adm", result.Username);
            _userRepositoryMock.Verify(r => r.GetByUsernameAsync("adm"), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedUser = new User { Id = id, Username = "user1", PasswordHash = "123", Email = "email@mail.com", Role = UserRole.Standard };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            _userRepositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser_WhenUsernameIsNew()
        {
            // Arrange
            var dto = new RegisterRequestDTO
            {
                Username = "newuser",
                Password = "123",
                Email = "test@email.com",
                Role = UserRole.Standard
            };

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(dto.Username)).ReturnsAsync((User?)null);
            _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<User>(), dto.Password))
                .Returns("hashedPassword");

            // Act
            var result = await _userService.CreateUserAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("newuser", result.Username);
            Assert.Equal("hashedPassword", result.PasswordHash);
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldThrow_WhenUsernameAlreadyExists()
        {
            // Arrange
            var dto = new RegisterRequestDTO { Username = "adm", Password = "123", Email = "email@mail.com", Role = UserRole.Standard };
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(dto.Username))
                .ReturnsAsync(new User { Username = "adm", Email = "email@mail.com", Role = UserRole.Standard, PasswordHash = string.Empty });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateUserAsync(dto));
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnUpdatedUser_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = new User { Id = id, Username = "olduser", Email = "old@mail.com", Role = UserRole.Admin, PasswordHash = string.Empty  };
            var dto = new UpdateUserDTO { Email = "new@mail.com", FullName = "New Name", Role = UserRole.Admin, IsActive = true };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

            // Act
            var result = await _userService.UpdateUserAsync(id, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new@mail.com", result.Email);
            Assert.Equal("New Name", result.FullName);
            _userRepositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new UpdateUserDTO { Email = "new@mail.com" };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.UpdateUserAsync(id, dto);

            // Assert
            Assert.Null(result);
            _userRepositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeactivateUserAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = new User { Id = id, Username = "teste", IsActive = true, Email = "email@email.com", Role = UserRole.Admin, PasswordHash = string.Empty  };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

            // Act
            var result = await _userService.DeactivateUserAsync(id);

            // Assert
            Assert.True(result);
            Assert.False(user.IsActive);
            _userRepositoryMock.Verify(r => r.Update(user), Times.Once);
            _userRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeactivateUserAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.DeactivateUserAsync(id);

            // Assert
            Assert.False(result);
            _userRepositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        }
    }
}
