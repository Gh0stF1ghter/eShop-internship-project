using Identity.BusinessLogic;
using Identity.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger) : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly ILogger<AuthenticationController> _logger = logger;

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            _logger.LogInformation("--- Init Login endpoint -------------");
            if(!ModelState.IsValid)
            {
                _logger.LogDebug("--- Login failed. Invalid Credentials -------------");
                return BadRequest("Invalid email or password " + ModelState);
            }

            var token = await _authenticationService.AuthenticateAsync(login);

            if (token is null)
            {
                _logger.LogDebug("--- Login failed. User Not Found or Invalid Password -------------");
                return BadRequest("Invalid email or password");
            }

            _logger.LogInformation("--- Login finished successfully -------------");
            return Ok(token);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            _logger.LogInformation("--- Init Register endpoint -------------");
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("--- Register failed. Invalid Credentials -------------");
                return BadRequest("Entered data is invalid " + ModelState);
            }

            var registered = await _authenticationService.RegisterUserAsync(register);

            if (!registered)
            {
                _logger.LogDebug("--- Register failed. User with this email exists or internal error -------------");

                return BadRequest("Something went wrong or user already exists");
            }

            _logger.LogInformation("--- Registration finished successfully -------------");
            return Ok("Successfully signed up");
        }
    }
}
