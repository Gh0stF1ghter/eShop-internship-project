﻿using AutoMapper;
using Baskets.BusinessLogic.Comands.BasketItem;
using Baskets.BusinessLogic.Queries.BasketItem;
using Baskets.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class UpdateBasketItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateBasketItemComand>
    {
        public async Task Handle(UpdateBasketItemComand comand, CancellationToken cancellationToken)
        {
            var basket = await FindBasket(comand, cancellationToken);

            var itemToUpdate = await FindInBasket(comand, cancellationToken);

            basket.TotalPrice -= itemToUpdate.Item.Price * itemToUpdate.Quantity;

            itemToUpdate.Quantity = comand.Quantity;
            itemToUpdate.SumPrice = itemToUpdate.Item.Price * comand.Quantity;

            unitOfWork.BasketItem
                .Update(bi => bi.Id.Equals(comand.BasketItemId), itemToUpdate);

            UpdateTotalCost(basket, itemToUpdate);
        }

        private async Task<UserBasket> FindBasket(UpdateBasketItemComand comand, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket
                .GetByConditionAsync(b => b.UserId.Equals(comand.UserId), cancellationToken);

            if (basket == null)
            {
                throw new BadRequestException(UserBasketMessages.NotFound);
            }

            return basket;
        }

        private async Task<BasketItem> FindInBasket(UpdateBasketItemComand comand, CancellationToken cancellationToken)
        {
            var itemInBasket = await unitOfWork.BasketItem
                .GetByConditionAsync(bi => bi.ItemId.Equals(comand.BasketItemId) 
                && bi.UserId.Equals(comand.UserId), cancellationToken);
            
            if (itemInBasket == null)
            {
                throw new BadRequestException(BasketItemMessages.NotInCurrentBasket);
            }

            return itemInBasket;
        }

        private void UpdateTotalCost(UserBasket basket, BasketItem newItem)
        {
            basket.TotalPrice += newItem.SumPrice;

            unitOfWork.Basket
                .Update(u => u.UserId.Equals(basket.UserId), basket);
        }
    }
}
