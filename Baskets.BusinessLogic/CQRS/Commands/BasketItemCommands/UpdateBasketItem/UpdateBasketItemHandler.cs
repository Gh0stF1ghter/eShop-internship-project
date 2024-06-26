﻿using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.UpdateBasketItem
{
    public class UpdateBasketItemHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateBasketItemCommand>
    {
        public async Task Handle(UpdateBasketItemCommand comand, CancellationToken cancellationToken)
        {
            var basket = await FindBasket(comand, cancellationToken);

            var itemToUpdate = await FindInBasket(comand, cancellationToken);

            basket.TotalPrice -= itemToUpdate.SumPrice;

            itemToUpdate.Quantity = comand.Quantity;
            itemToUpdate.SumPrice = itemToUpdate.Item.Price * comand.Quantity;

            await unitOfWork.BasketItem
                .UpdateAsync(bi => bi.BasketItemId.Equals(comand.BasketItemId), itemToUpdate, cancellationToken);

            await UpdateTotalCost(basket, itemToUpdate, cancellationToken);
        }

        private async Task<UserBasket> FindBasket(UpdateBasketItemCommand comand, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket
                .GetByConditionAsync(b => b.UserId.Equals(comand.UserId), cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException(UserBasketMessages.NotFound);
            }

            return basket;
        }

        private async Task<BasketItem> FindInBasket(UpdateBasketItemCommand comand, CancellationToken cancellationToken)
        {
            var itemInBasket = await unitOfWork.BasketItem
                .GetBasketItemByConditionAsync(bi => bi.BasketItemId.Equals(comand.BasketItemId)
                && bi.UserId.Equals(comand.UserId), cancellationToken);

            if (itemInBasket == null)
            {
                throw new NotFoundException(BasketItemMessages.NotFound);
            }

            return itemInBasket;
        }

        private async Task UpdateTotalCost(UserBasket basket, BasketItem newItem, CancellationToken cancellationToken)
        {
            basket.TotalPrice += newItem.SumPrice;

            await unitOfWork.Basket
                .UpdateAsync(u => u.UserId.Equals(basket.UserId), basket, cancellationToken);
        }
    }
}