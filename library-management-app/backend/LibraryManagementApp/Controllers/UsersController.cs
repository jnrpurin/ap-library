using LibraryManagementApp.DTO;
using LibraryManagementApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Controllers
{
    /// <summary>
    /// Controller to manage Users
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Ok with the users</returns>
        [HttpGet]
        [Authorize(Roles = "User_Admin")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all users...");
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get user by your Guid identificator
        /// </summary>
        /// <param name="id">Guid identificator</param>
        /// <returns>Ok with the user if exists</returns>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "User_Admin,User_Standard")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching user with ID: {UserId}", id);
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Method to create a new user.
        /// </summary>
        /// <param name="dto">Strings Username, Password and Email, FullName is optional</param>
        /// <returns>OK if success or BadRequest if failed to register</returns>
        [HttpPost]
        [Authorize(Roles = "User_Admin")]
        public async Task<IActionResult> Create([FromBody] RegisterRequestDTO dto)
        {
            try
            {
                _logger.LogInformation("Creating new user: {Username}", dto.Username);
                var user = await _userService.CreateUserAsync(dto);
                _logger.LogInformation("User {UserId} created successfully", user.Id);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user {Username}", dto.Username);
                return StatusCode(500, "An internal error occurred while creating the user.");
            }
        }

        /// <summary>
        /// Update an user record
        /// </summary>
        /// <param name="id">User guid identificator</param>
        /// <param name="dto">User email, full name, role and if it is active</param>
        /// <returns>Ok with the user updated data</returns>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "User_Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDTO dto)
        {
            try
            {
                _logger.LogInformation("Updating user with ID: {UserId}", id);
                var updated = await _userService.UpdateUserAsync(id, dto);
                if (updated == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found for update", id);
                    return NotFound();
                }

                _logger.LogInformation("User {UserId} updated successfully", id);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                return StatusCode(500, "An internal error occurred while updating the user.");
            }
        }

        /// <summary>
        /// Deactive an user
        /// </summary>
        /// <param name="id">User guid id</param>
        /// <returns>Ok for deactivated user</returns>
        [HttpPatch("{id:guid}/deactivate")]
        [Authorize(Roles = "User_Admin")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            try
            {
                _logger.LogInformation("Deactivating user with ID: {UserId}", id);
                var result = await _userService.DeactivateUserAsync(id);
                if (!result)
                {
                    _logger.LogWarning("User with ID {UserId} not found for deactivation", id);
                    return NotFound();
                }

                _logger.LogInformation("User {UserId} deactivated successfully", id);
                return Ok(new { message = "User deactivated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating user {UserId}", id);
                return StatusCode(500, "An internal error occurred while deactivating the user.");
            }
        }
    }
}
