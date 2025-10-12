using LibraryManagementApp.DTO;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementApp.Services
{
    public class UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

        public async Task<IEnumerable<User>> GetAllUsersAsync()
            => await _userRepository.GetAllAsync();

        public async Task<User?> GetUserByUsernameAsync(string Username)
            => await _userRepository.GetByUsernameAsync(Username);

        public async Task<User?> GetUserByIdAsync(Guid id)
            => await _userRepository.GetByIdAsync(id);

        public async Task<User> CreateUserAsync(RegisterRequestDTO dto)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                FullName = dto.FullName ?? string.Empty,
                PasswordHash = string.Empty,
                Role = dto.Role
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<User?> UpdateUserAsync(Guid id, UpdateUserDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            user.FullName = dto.FullName ?? user.FullName;
            user.Email = dto.Email ?? user.Email;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeactivateUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.IsActive = false;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }
    }
}
