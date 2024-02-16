using Identity.BusinessLogic.DTOs;
using Identity.DataAccess.Models;
using System.Security.Claims;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims);
        Task<TokenDTO> CreateTokenAsync(User user, bool populateExp);
        Task<TokenDTO> RefreshTokenAsync(TokenDTO tokenDto);
    }
}
