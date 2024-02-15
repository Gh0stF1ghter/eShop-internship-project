using System.Security.Claims;

namespace Identity.BusinessLogic.Services
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}
