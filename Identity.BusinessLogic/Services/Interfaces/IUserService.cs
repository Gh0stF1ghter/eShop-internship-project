using Identity.BusinessLogic.DTOs;

namespace Identity.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken);

        Task<UserDto> GetUserByIdAsync(string id, CancellationToken cancellationToken);

        Task DeleteUserAsync(string id, CancellationToken cancellationToken);
    }
}