using Baskets.BusinessLogic.Comands.CustomerBasket;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.Handlers.CustomerBasketHandlers
{
    public class DeleteUserBasketHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserBasketComand>
    {
        public async Task Handle(DeleteUserBasketComand command, CancellationToken cancellationToken)
        {
            await FindUser(command, cancellationToken);

            var basket = await unitOfWork.Basket
                .DeleteAsync(b => b.UserId.Equals(command.UserId), cancellationToken);

            if (basket == null)
            {
                throw new BadRequestException(UserBasketMessages.NotFound);
            }
        }

        public async Task FindUser(DeleteUserBasketComand command, CancellationToken cancellationToken)
        {
            var userExists = await unitOfWork.User
                .GetByConditionAsync(u => u.Id.Equals(command.UserId), cancellationToken);

            if (userExists != null)
            {
                throw new BadRequestException(UserBasketMessages.UserExists);
            }
        }
    }
}