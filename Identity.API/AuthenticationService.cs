using Identity.BusinessLogic;
using Identity.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.API
{
    public class AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthenticationService> logger) : IAuthenticationService
    {
        public readonly UserManager<User> _userManager = userManager;
        public readonly RoleManager<IdentityRole> _roleManager = roleManager;
        public readonly IConfiguration _configuration = configuration;

        public readonly ILogger<AuthenticationService> _logger = logger;

        public async Task<string?> AuthenticateAsync(Login loginCredentials)
        {
            _logger.LogInformation("Searching for user with email {email} ", loginCredentials.Email);
            var user = await _userManager.FindByEmailAsync(loginCredentials.Email);


            if (user is null)
            {
                _logger.LogDebug("No User");
                return null;
            }


            _logger.LogInformation("User: {id} {email} {name}", user.Id, user.Email, user.UserName);
            var IsPasswordValid = await _userManager.CheckPasswordAsync(user, loginCredentials.Password);
            if (!IsPasswordValid)
            {
                _logger.LogDebug("Invalid Password");
                return null;
            }

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

            var token = GenerateToken(userClaims);

            return token;
        }

        public async Task<bool> RegisterUserAsync(Register registerCredentials)
        {


            _logger.LogDebug("Searching for user with email {email} ", registerCredentials.Email);
            var userExists = await _userManager.FindByEmailAsync(registerCredentials.Email);

            if (userExists is not null)
            {
                _logger.LogDebug("User found: {id} {name} {email}", userExists.Id, userExists.UserName, userExists.Email);
                return false;
            }

            var user = new User
            {
                UserName = registerCredentials.Username,
                Email = registerCredentials.Email
            };

            _logger.LogInformation("Creating new user {name} {email}", user.UserName, user.Email);
            var createUser = await _userManager.CreateAsync(user, registerCredentials.Password);
            if (!createUser.Succeeded)
            {
                _logger.LogError("Internal error while creating new user {errrors}", createUser.Errors);
                return false;
            }

            _logger.LogInformation("Adding role {role} to user {name}", Roles.User, user.UserName);
            await _userManager.AddToRoleAsync(user, Roles.User);

            return true;
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = new(signingKey, SecurityAlgorithms.HmacSha256),
                Subject = new(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
