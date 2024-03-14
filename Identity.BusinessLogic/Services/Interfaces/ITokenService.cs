using Identity.BusinessLogic.DTOs;
using Identity.DataAccess.Entities.Models;
using System.Security.Claims;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDTO> CreateTokenAsync(User user, bool populateExp);
        Task<TokenDTO> RefreshTokenAsync(TokenDTO tokenDto, CancellationToken token);
    }
}
