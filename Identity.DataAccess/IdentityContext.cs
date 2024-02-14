using Identity.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Identity.Data
{
    public class IdentityContext : IdentityDbContext<User>
    {

    }
}
