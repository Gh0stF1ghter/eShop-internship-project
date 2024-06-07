using AutoMapper;
using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.CreateUserBasket
{
    public class CreateUserBasketHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CreateUserBasketCommand, UserBasketDto>
    {
        public async Task<UserBasketDto> Handle(CreateUserBasketCommand comand, CancellationToken cancellationToken)
        {
            await FindReferencesAsync(comand, cancellationToken);

            var newBasket = new UserBasket
            {
                UserId = comand.UserId
            };

            await unitOfWork.Basket.AddAsync(newBasket, cancellationToken);

            var newBasketDto = mapper.Map<UserBasket, UserBasketDto>(newBasket);

            return newBasketDto;
        }

        private async Task FindReferencesAsync(CreateUserBasketCommand comand, CancellationToken cancellationToken)
        {
            var basketExists = await unitOfWork.Basket
                .GetByConditionAsync(b => b.UserId.Equals(comand.UserId), cancellationToken);

            if (basketExists != null)
            {
                throw new BadRequestException(UserBasketMessages.Exists);
            }
        }
    }
}