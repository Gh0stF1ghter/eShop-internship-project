using Identity.DataAccess.Data;
using Identity.DataAccess.Entities.Models;
using Identity.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Repositories.Implementations
{
    public class UserRepository(IdentityContext context) : IUserRepository
    {
        public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var user = await context.Users.ToListAsync(cancellationToken);

            return user;
        }

        public async Task<User?> GetUserByIdAsync(string id, CancellationToken cancellationToken)
        {
            var user = await context.Users.Where(u => u.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);

            return user;
        }

        public async Task DeleteAsync(User user, CancellationToken cancellationToken)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}