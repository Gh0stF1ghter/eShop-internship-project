using Identity.BusinessLogic.DTOs;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenDTO?> AuthenticateAsync(LoginDTO loginCredentials);
        Task<bool> RegisterUserAsync(RegisterDTO registerCredentials);
    }
}
