using System.Security.Claims;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}
