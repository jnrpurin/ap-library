using LibraryManagementApp.DTO;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Controllers
{
    /// <summary>
    /// API controller for managing users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public UserLoginController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        /// <summary>
        /// Method to login a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request");

            var user = await _authService.GetUserByUsernameAsync(request.Username);
            if (user == null)
                return Unauthorized("Invalid username or password");

            var isValid = await _authService.VerifyLoginAsync(request.Username, request.Password);
            if (!isValid)
                return Unauthorized("Invalid username or password");

            var token = _authService.GenerateJwtToken(user, _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"]);

            return Ok(new
            {
                message = "Login successful",
                token = $"Bearer {token}",
                user = new
                {
                    user.Id,
                    user.Username
                }
            });
        }

        /// <summary>
        /// Method to register a new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            try
            {
                var user = await _authService.RegisterAsync(request);
                return Ok(new { message = "User created successfully", userId = user.Id });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}