using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;


namespace Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.DeleteUserBasket
{
    public class DeleteUserBasketHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserBasketCommand>
    {
        public async Task Handle(DeleteUserBasketCommand command, CancellationToken cancellationToken)
        {
            await FindUserAsync(command, cancellationToken);

            var basket = await unitOfWork.Basket
                .DeleteAsync(b => b.UserId.Equals(command.UserId), cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException(UserBasketMessages.NotFound);
            }
        }

        public async Task FindUserAsync(DeleteUserBasketCommand command, CancellationToken cancellationToken)
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