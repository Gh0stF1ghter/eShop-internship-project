using Identity.DataAccess.Entities.Models;

namespace Identity.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);

        Task<User?> GetUserByIdAsync(string id, CancellationToken cancellationToken);

        Task DeleteAsync(User user, CancellationToken cancellationToken);
    }
}