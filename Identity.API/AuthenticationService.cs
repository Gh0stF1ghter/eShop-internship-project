using Identity.BusinessLogic;
using Identity.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.API
{
    public class AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : IAuthenticationService
    {
        public readonly UserManager<User> _userManager = userManager;
        public readonly RoleManager<IdentityRole> _roleManager = roleManager;
        public readonly IConfiguration _configuration = configuration;

        public async Task<string?> AuthenticateAsync(Login loginCredentials)
        {
            var user = await _userManager.FindByEmailAsync(loginCredentials.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, loginCredentials.Password))
                return null;

            var userRoles = await _userManager.GetRolesAsync(user);

            var userClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if(user.UserName is not null)
                userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));

            foreach (var role in userRoles)
                userClaims.Add(new Claim(ClaimTypes.Role, role));

            var token = GenerateToken(userClaims);

            return token;
        }

        public async Task<bool> RegisterUserAsync(Register registerCredentials)
        {
            var userExists = await _userManager.FindByEmailAsync(registerCredentials.Email);
            if(userExists is not null)
                return false;

            var user = new User
            {
                UserName = registerCredentials.Username,
                Email = registerCredentials.Email
            };

            var createUser = await _userManager.CreateAsync(user, registerCredentials.Password);
            if(!createUser.Succeeded)
                return false;

            await _userManager.AddToRoleAsync(user, Roles.User);

            return true;
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Authentication:Issuer"],
                Audience = _configuration["Authentication:Audience"],
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
