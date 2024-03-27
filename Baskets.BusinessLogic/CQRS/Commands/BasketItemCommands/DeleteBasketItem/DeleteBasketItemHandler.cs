using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.DeleteBasketItem
{
    public class DeleteBasketItemHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBasketItemCommand>
    {
        public async Task Handle(DeleteBasketItemCommand comand, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket
                .GetByConditionAsync(u => u.UserId.Equals(comand.UserId), cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException(UserBasketMessages.NotFound);
            }

            var basketItem = await unitOfWork.BasketItem
                .DeleteAsync(bi => bi.BasketItemId.Equals(comand.ItemId)
                && bi.UserId.Equals(comand.UserId), cancellationToken);

            if (basketItem == null)
            {
                throw new NotFoundException(BasketItemMessages.NotFound);
            }

            await UpdateBasket(basket, basketItem, cancellationToken);
        }

        private async Task UpdateBasket(UserBasket basket, BasketItem basketItem, CancellationToken cancellationToken)
        {
            basket.TotalPrice -= basketItem.SumPrice;

            await unitOfWork.Basket
                .UpdateAsync(u => u.UserId.Equals(basket.UserId), basket, cancellationToken);
        }
    }
}