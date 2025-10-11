using LibraryManagementApp.DTO;
using LibraryManagementApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Controllers
{
    /// <summary>
    /// API controller for managing users.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController(IAuthService authService, IUserService userService, IConfiguration configuration) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IUserService _userService = userService;
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Method to login a user.
        /// </summary>
        /// <param name="request">String Username and Password</param>
        /// <returns>Error for invalid data, Ok with the token for valid login</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request");

            var user = await _userService.GetUserByUsernameAsync(request.Username);
            if (user == null)
                return Unauthorized("Invalid username or password");

            var isValid = await _authService.VerifyLoginAsync(request.Username, request.Password);
            if (!isValid)
                return Unauthorized("Invalid username or password");

            var secretKey = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                return StatusCode(500, "JWT configuration is missing or invalid");

            var token = _authService.GenerateJwtToken(user, secretKey, issuer, audience);

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
    }
}