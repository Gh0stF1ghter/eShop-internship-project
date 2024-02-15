using Identity.Core.Models;

namespace Identity.BusinessLogic
{
    public interface IAuthenticationService
    {
        Task<string?> AuthenticateAsync(Login loginCredentials);
        Task<bool> RegisterUserAsync(Register registerCredentials);
    }
}
