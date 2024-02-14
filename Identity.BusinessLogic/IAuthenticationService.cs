using Identity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLogic
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAsync(Login loginCredentials);
        Task<bool> RegisterUserAsync(Register registerCredentials);
        Task<bool> RegisterAdminAsync(Register registerCredentials);

        string GenerateToken(IEnumerable<Claim> claims);
    }
}
