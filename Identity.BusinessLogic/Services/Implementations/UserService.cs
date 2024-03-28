using AutoMapper;
using Identity.BusinessLogic.DTOs;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities.Constants;
using Identity.DataAccess.Entities.Exceptions;
using Identity.DataAccess.Repositories.Interfaces;

namespace Identity.BusinessLogic.Services.Implementations
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllUsersAsync(cancellationToken);

            var usersDto = mapper.Map<IEnumerable<UserDto>>(users);

            return usersDto;
        }

        public async Task<UserDto> GetUserByIdAsync(string id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByIdAsync(id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(UserMessages.UserNotFound);
            }

            var userDto = mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task DeleteUserAsync(string id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByIdAsync(id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(UserMessages.UserNotFound);
            }

            await userRepository.DeleteAsync(user, cancellationToken);
        }
    }
}