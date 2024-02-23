using Identity.BusinessLogic.DTOs;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities.Constants;
using Identity.DataAccess.Entities.Exceptions;
using Identity.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class TokenService(IConfiguration configuration, UserManager<User> userManager) : ITokenService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;

        public async Task<TokenDTO> CreateTokenAsync(User user, bool populateExp)
        {
            var claims = await ClaimRoles(user);

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;

            var accessToken = GenerateToken(claims);

            if (populateExp)
            {
                user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
            }

            await _userManager.UpdateAsync(user);

            return new TokenDTO(accessToken, refreshToken);
        }

        public string GenerateToken(IEnumerable<Claim> claims)
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

        public async Task<TokenDTO> RefreshTokenAsync(TokenDTO tokenDto, CancellationToken token)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
            {
                throw new RefreshTokenBadRequestException(Messages.InvalidToken);
            }

            return await CreateTokenAsync(user, populateExp: false);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumer = new byte[32];

            using var numberGenerator = RandomNumberGenerator.Create();
            numberGenerator.GetBytes(randomNumer);

            return Convert.ToBase64String(randomNumer);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            return principal;
        }

        private async Task<List<Claim>> ClaimRoles(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var userClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (user.UserName is not null)
            {
                userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            }

            userRoles
                .ToList()
                .ForEach(ur => userClaims.Add(new Claim(ClaimTypes.Role, ur)));

            return userClaims;
        }
    }
}