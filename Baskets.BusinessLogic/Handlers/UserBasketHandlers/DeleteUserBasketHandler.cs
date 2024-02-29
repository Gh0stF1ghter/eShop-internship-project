using AutoMapper;
using Baskets.BusinessLogic.Comands.CustomerBasket;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.Handlers.CustomerBasketHandlers
{
    public class DeleteUserBasketHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserBasketComand>
    {
        public async Task Handle(DeleteUserBasketComand command, CancellationToken cancellationToken)
        {
            var userExists = await unitOfWork.User.GetByConditionAsync(u => u.UserId.Equals(command.UserId), cancellationToken);

            if (userExists != null)
            {
                throw new BadRequestException(UserBasketMessages.UserExists);
            }

            var basket = await unitOfWork.Basket.GetByConditionAsync(b => b.UserId.Equals(command.UserId), cancellationToken);

            if (basket == null)
            {
                throw new BadRequestException(UserBasketMessages.NotFound);
            }

            unitOfWork.Basket.Delete(basket);
        }
    }
}
