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
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IAuthService authService, IUserService userService, IConfiguration configuration, ILogger<LoginController> logger)
        {
            _authService = authService;
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Method to login a user.
        /// </summary>
        /// <param name="request">String Username and Password</param>
        /// <returns>Error for invalid data, Ok with the token for valid login</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            _logger.LogInformation("Login attempt for user: {Username}", request.Username);

            if (!ModelState.IsValid)
                return BadRequest("Invalid request");

            try
            {
                var user = await _userService.GetUserByUsernameAsync(request.Username);
                if (user == null)
                {
                    _logger.LogWarning("Invalid username: {Username}", request.Username);
                    return Unauthorized("Invalid username or password");
                }

                var isValid = await _authService.VerifyLoginAsync(request.Username, request.Password);
                if (!isValid)
                {
                    _logger.LogWarning("Invalid password for user: {Username}", request.Username);
                    return Unauthorized("Invalid username or password");
                }

                var secretKey = _configuration["Jwt:Key"];
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];

                if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                {
                    _logger.LogCritical("JWT configuration missing");
                    return StatusCode(500, "JWT configuration is missing or invalid");
                }

                var token = _authService.GenerateJwtToken(user, secretKey, issuer, audience);

                _logger.LogInformation("User {Username} logged in successfully", request.Username);
                return Ok(new
                {
                    message = "Login successful",
                    token = $"Bearer {token}",
                    userName = user.Username,
                    roles = user.Role
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for {Username}", request.Username);
                return StatusCode(500, "An internal error occurred while processing the login.");
            }
        }
    }
}