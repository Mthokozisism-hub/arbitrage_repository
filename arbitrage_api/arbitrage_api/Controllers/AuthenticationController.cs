using arbitrage_api.Domains.Users;
using arbitrage_api.Services.AuthTokenServices;
using arbitrage_api.Services.AuthTokenServices.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace arbitrage_api.Controllers
{
    // Marks this class as an API controller
    [ApiController]
    // Defines the base route for all endpoints in this controller
    [Route("Api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        // Manages user-related operations (e.g., creating users, finding users)
        private readonly UserManager<User> _userManager;

        // Manages user sign-in operations (e.g., password validation)
        private readonly SignInManager<User> _signInManager;

        // Service for generating JWT tokens
        private readonly IAuthTokenService _authTokenService;

        // Constructor for dependency injection
        public AuthenticationController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAuthTokenService authTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authTokenService = authTokenService;
        }

        // Endpoint for user registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // Create a new User object with the provided email and username
            var user = new User { UserName = model.Email, Email = model.Email };

            // Attempt to create the user with the provided password
            var result = await _userManager.CreateAsync(user, model.Password);

            // If the user creation is successful, return a success message
            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully" });
            }

            // If user creation fails, return the errors
            return BadRequest(result.Errors);
        }

        // Endpoint for user login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // Attempt to sign in the user with the provided email and password
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            // If the login is successful, generate a JWT token for the user
            if (result.Succeeded)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // Generate a JWT token for the user
                var token = _authTokenService.GenerateToken(user);

                // Return the token in the response
                return Ok(new { Token = token });
            }

            // If login fails, return an unauthorized response
            return Unauthorized();
        }
    }
}