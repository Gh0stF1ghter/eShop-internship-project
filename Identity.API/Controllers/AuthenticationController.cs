using Identity.BusinessLogic.DTOs;
using Identity.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login, CancellationToken token = default)
        {
            var tokens = await _authenticationService.AuthenticateAsync(login, token);

            return Ok(tokens);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO register, CancellationToken token = default)
        {
            await _authenticationService.RegisterUserAsync(register, token);

            return Created();
        }
    }
}
