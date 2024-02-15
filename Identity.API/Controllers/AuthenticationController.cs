using Identity.BusinessLogic.DTOs;
using Identity.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login)
        {
            var token = await _authenticationService.AuthenticateAsync(login);

            return Ok(token);
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO register)
        {
            await _authenticationService.RegisterUserAsync(register);

            return Created();
        }
    }
}
