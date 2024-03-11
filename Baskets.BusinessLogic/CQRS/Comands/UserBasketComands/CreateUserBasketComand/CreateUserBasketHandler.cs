using AutoMapper;
using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Comands.UserBasketComands.CreateUserBasketComand
{
    public class CreateUserBasketHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateUserBasketComand, UserBasketDto>
    {
        public async Task<UserBasketDto> Handle(CreateUserBasketComand comand, CancellationToken cancellationToken)
        {
            await FindReferencesAsync(comand, cancellationToken);

            var newBasket = new UserBasket
            {
                UserId = comand.UserId
            };

            unitOfWork.Basket.Add(newBasket);

            var newBasketDto = mapper.Map<UserBasket, UserBasketDto>(newBasket);

            return newBasketDto;
        }

        private async Task FindReferencesAsync(CreateUserBasketComand comand, CancellationToken cancellationToken)
        {
            var userExists = await unitOfWork.User
                .GetByConditionAsync(u => u.Id.Equals(comand.UserId), cancellationToken);

            if (userExists == null)
            {
                throw new NotFoundException(UserMessages.NotFound);
            }

            var basketExists = await unitOfWork.Basket
                .GetByConditionAsync(b => b.UserId.Equals(comand.UserId), cancellationToken);

            if (basketExists != null)
            {
                throw new NotFoundException(UserBasketMessages.Exists);
            }
        }
    }
}