using Baskets.DataAccess.UnitOfWork;
using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.Entities.Models;


namespace Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.DeleteBasketItemComand
{
    public class DeleteBasketItemHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBasketItemComand>
    {
        public async Task Handle(DeleteBasketItemComand comand, CancellationToken cancellationToken)
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