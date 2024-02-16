using Identity.BusinessLogic.DTOs;
using Identity.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController(ITokenService tokenService) : ControllerBase
    {
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync([FromBody] TokenDTO tokenDTO)
        {
            var tokens = await _tokenService.RefreshTokenAsync(tokenDTO);

            return Ok(tokens);
        }
    }
}
