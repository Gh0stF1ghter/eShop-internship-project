using Baskets.BusinessLogic.Comands.BasketItem;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class DeleteBasketItemHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBasketItemComand>
    {
        public async Task Handle(DeleteBasketItemComand comand, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket
                .GetByConditionAsync(u => u.UserId.Equals(comand.UserId), cancellationToken);

            if (basket == null)
            {
                throw new BadRequestException(UserBasketMessages.NotFound);
            }

            var basketItem = await unitOfWork.BasketItem
                .DeleteAsync(bi => bi.Id.Equals(comand.ItemId)
                && bi.UserId.Equals(comand.UserId), cancellationToken);

            if (basketItem == null)
            {
                throw new BadRequestException(BasketItemMessages.NotFound);
            }

            UpdateBasket(basket, basketItem);
        }

        private void UpdateBasket(UserBasket basket, BasketItem basketItem)
        {
            basket.TotalPrice -= basketItem.SumPrice;

            unitOfWork.Basket
                .Update(u => u.UserId.Equals(basket.UserId), basket);
        }
    }
}