using Identity.Core.Models;
using System.Security.Claims;

namespace Identity.BusinessLogic
{
    public interface IAuthenticationService
    {
        Task<string?> AuthenticateAsync(Login loginCredentials);
        Task<bool> RegisterUserAsync(Register registerCredentials);
    }
}
