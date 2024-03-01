using Baskets.BusinessLogic.Comands.BasketItem;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class DeleteBasketItemHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBasketItemComand>
    {
        public async Task Handle(DeleteBasketItemComand comand, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.User.GetByConditionAsync(u => u.Id.Equals(comand.UserId), cancellationToken);

            if (user == null)
            {
                throw new BadRequestException(UserMessages.NotFound);
            }

            var basketItem = await unitOfWork.BasketItem.DeleteAsync(bi => bi.Id.Equals(comand.ItemId) && bi.UserId.Equals(comand.UserId), cancellationToken);

            if (basketItem == null)
            {
                throw new BadRequestException(BasketItemMessages.NotFound);
            }
        }
    }
}
