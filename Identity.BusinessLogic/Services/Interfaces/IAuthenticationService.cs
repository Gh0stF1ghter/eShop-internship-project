using Identity.BusinessLogic.DTOs;
using Identity.DataAccess.Entities.Models;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenDTO?> AuthenticateAsync(LoginDTO loginCredentials, CancellationToken token);
        Task<User> RegisterUserAsync(RegisterDTO registerCredentials, CancellationToken token);
    }
}
