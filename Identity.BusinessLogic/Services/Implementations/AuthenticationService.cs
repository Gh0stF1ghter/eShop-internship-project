using AutoMapper;
using Identity.BusinessLogic.DTOs;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities.Constants;
using Identity.DataAccess.Entities.Exceptions;
using Identity.DataAccess.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class AuthenticationService(
        UserManager<User> userManager,
        ITokenService tokenService,
        IMapper mapper,
        ILogger<AuthenticationService> logger
        ) : IAuthenticationService
    {
        private readonly UserManager<User> _userManager = userManager;

        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AuthenticationService> _logger = logger;

        public async Task<TokenDTO?> AuthenticateAsync(LoginDTO loginCredentials, CancellationToken cancellationToken)
        {
            User user = await ValidateAuthenticationAsync(loginCredentials.Email, loginCredentials.Password);

            var tokenDto = await _tokenService.CreateTokenAsync(user, populateExp: true);

            return tokenDto;
        }

        public async Task<bool> RegisterUserAsync(RegisterDTO registerCredentials, CancellationToken token)
        {
            await ValidateRegisterAsync(registerCredentials.Email);

            var user = _mapper.Map<User>(registerCredentials);

            await CreateUserAsync(user, registerCredentials.Password);

            _logger.LogInformation("Adding role {role} to user {name}", UserRoles.User, user.UserName);

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            return true;
        }

        private async Task<User> ValidateAuthenticationAsync(string email, string password)
        {
            _logger.LogInformation("Searching for user with email {email} ", email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogDebug("No User");
                throw new BadRequestException(Messages.NoUser);
            }

            _logger.LogInformation("User: {id} {email} {name}", user.Id, user.Email, user.UserName);
            var IsPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!IsPasswordValid)
            {
                _logger.LogDebug("Invalid Password");
                throw new BadRequestException(Messages.InvalidPassword);
            }

            return user;
        }

        private async Task ValidateRegisterAsync(string email)
        {
            _logger.LogDebug("Searching for user with email {email} ", email);
            var emailTaken = await _userManager.FindByEmailAsync(email);

            if (emailTaken is not null)
            {
                _logger.LogDebug("User found: {id} {name} {email}", emailTaken.Id, emailTaken.UserName, emailTaken.Email);
                throw new BadRequestException(Messages.EmailTaken);
            }
        }

        private async Task CreateUserAsync(User user, string password)
        {
            _logger.LogInformation("Creating new user {name} {email}", user.UserName, user.Email);
            var createUser = await _userManager.CreateAsync(user, password);

            if (!createUser.Succeeded)
            {
                _logger.LogError("Error while creating new user {errors}", createUser.Errors);
                throw new BadRequestException(Messages.RegisterFailed);
            }
        }
    }
}
