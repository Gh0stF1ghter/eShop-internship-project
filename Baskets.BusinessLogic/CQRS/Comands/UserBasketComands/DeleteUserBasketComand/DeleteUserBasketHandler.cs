using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;


namespace Baskets.BusinessLogic.CQRS.Comands.UserBasketComands.DeleteUserBasketComand
{
    public class DeleteUserBasketHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserBasketComand>
    {
        public async Task Handle(DeleteUserBasketComand command, CancellationToken cancellationToken)
        {
            await FindUserAsync(command, cancellationToken);

            var basket = await unitOfWork.Basket
                .DeleteAsync(b => b.UserId.Equals(command.UserId), cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException(UserBasketMessages.NotFound);
            }
        }

        public async Task FindUserAsync(DeleteUserBasketComand command, CancellationToken cancellationToken)
        {
            var userExists = await unitOfWork.User
                .GetByConditionAsync(u => u.Id.Equals(command.UserId), cancellationToken);

            if (userExists != null)
            {
                throw new AlreadyExistsException(UserBasketMessages.UserExists);
            }
        }
    }
}