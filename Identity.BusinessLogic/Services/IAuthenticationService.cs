using Identity.BusinessLogic.DTOs;

namespace Identity.BusinessLogic.Services
{
    public interface IAuthenticationService
    {
        Task<string?> AuthenticateAsync(LoginDTO loginCredentials);
        Task<bool> RegisterUserAsync(RegisterDTO registerCredentials);
    }
}
