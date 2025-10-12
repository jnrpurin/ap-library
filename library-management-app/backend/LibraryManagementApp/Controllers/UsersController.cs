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
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Ok with the users</returns>
        [HttpGet]
        [Authorize(Roles = "User_Admin")]
        public async Task<IActionResult> GetAll()
        {
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
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
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
            var user = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
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
            var updated = await _userService.UpdateUserAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
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
            var result = await _userService.DeactivateUserAsync(id);
            if (!result) return NotFound();
            return Ok(new { message = "User deactivated successfully" });
        }
    }
}
