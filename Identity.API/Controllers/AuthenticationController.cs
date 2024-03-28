﻿using Identity.BusinessLogic.DTOs;
using Identity.BusinessLogic.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.EventBus;

namespace Identity.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authenticationService, IPublishEndpoint publishEndpoint) : ControllerBase
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
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO register, CancellationToken token = default)
        {
            var user = await _authenticationService.RegisterUserAsync(register, token);

            await publishEndpoint.Publish<UserCreated>(new(UserId: user.Id), token);
            await Console.Out.WriteLineAsync(user.Id + " published");

            return Created();
        }
    }
}