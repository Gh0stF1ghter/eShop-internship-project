using AutoMapper;
using Identity.BusinessLogic.DTOs;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities.Constants;
using Identity.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class EShopAuthenticationService(UserManager<User> userManager, ITokenService tokenService, IMapper mapper, ILogger<EShopAuthenticationService> logger) : IAuthenticationService
    {
        private readonly UserManager<User> _userManager = userManager;

        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<EShopAuthenticationService> _logger = logger;

        public async Task<string?> AuthenticateAsync(LoginDTO loginCredentials)
        {
            User user = await ValidateAuthenticationAsync(loginCredentials.Email, loginCredentials.Password);

            var userClaims = await ClaimRoles(user);

            var token = _tokenService.GenerateToken(userClaims);

            return token;
        }

        public async Task<bool> RegisterUserAsync(RegisterDTO registerCredentials)
        {
            await ValidateRegisterAsync(registerCredentials.Email, registerCredentials.Username);

            var user = _mapper.Map<User>(registerCredentials);

            await CreateUserAsync(user, registerCredentials.Password);

            _logger.LogInformation("Adding role {role} to user {name}", Roles.User, user.UserName);
            await _userManager.AddToRoleAsync(user, Roles.User);

            return true;
        }

        private async Task<User> ValidateAuthenticationAsync(string email, string password)
        {
            _logger.LogInformation("Searching for user with email {email} ", email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogDebug("No User");
                throw new BadRequestException("User does not exist");
            }

            _logger.LogInformation("User: {id} {email} {name}", user.Id, user.Email, user.UserName);
            var IsPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!IsPasswordValid)
            {
                _logger.LogDebug("Invalid Password");
                throw new BadRequestException("Invalid Password");
            }

            return user;
        }

        private async Task ValidateRegisterAsync(string email, string username)
        {
            _logger.LogDebug("Searching for user with email {email} ", email);
            var emailTaken = await _userManager.FindByEmailAsync(email);

            if (emailTaken is not null)
            {
                _logger.LogDebug("User found: {id} {name} {email}", emailTaken.Id, emailTaken.UserName, emailTaken.Email);
                throw new BadRequestException("Email already taken");
            }

            var userNameTaken = await _userManager.FindByNameAsync(username);

            if (userNameTaken is not null)
            {
                _logger.LogDebug("User found: {id} {name} {email}", userNameTaken.Id, userNameTaken.UserName, userNameTaken.Email);
                throw new BadRequestException("Username already taken");
            }
        }

        private async Task<List<Claim>> ClaimRoles(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var userClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (user.UserName is not null)
                userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));

            foreach (var role in userRoles)
            {
                _logger.LogInformation("User roles: {role}", role);
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            return userClaims;
        }

        private async Task CreateUserAsync(User user, string password)
        {
            _logger.LogInformation("Creating new user {name} {email}", user.UserName, user.Email);
            var createUser = await _userManager.CreateAsync(user, password);

            if (!createUser.Succeeded)
            {
                _logger.LogError("Internal error while creating new user {errrors}", createUser.Errors);
                throw new InvalidOperationException();
            }
        }
    }
}
