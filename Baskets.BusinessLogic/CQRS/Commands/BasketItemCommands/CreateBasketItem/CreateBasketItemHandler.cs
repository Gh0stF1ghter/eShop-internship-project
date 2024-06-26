﻿using AutoMapper;
using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.CreateBasketItem
{
    public class CreateBasketItemHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CreateBasketItemCommand, BasketItemDto>
    {
        public async Task<BasketItemDto> Handle(CreateBasketItemCommand comand, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.Item
                .GetByConditionAsync(i => i.Id.Equals(comand.CreateBasketItemDto.ItemId), cancellationToken);

            if (item == null)
            {
                throw new NotFoundException(ItemMessages.NotFound);
            }

            var basket = await unitOfWork.Basket
                .GetByConditionAsync(u => u.UserId.Equals(comand.UserId), cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException(UserMessages.NotFound);
            }

            await FindInBasketAsync(comand, cancellationToken);

            var newItem = mapper.Map<BasketItem>(comand.CreateBasketItemDto);

            newItem.Item = item;
            newItem.UserId = comand.UserId;
            newItem.SumPrice = item.Price;

            await unitOfWork.BasketItem.AddAsync(newItem, cancellationToken);

            await UpdateTotalCost(basket, newItem, cancellationToken);

            var newItemDto = mapper.Map<BasketItemDto>(newItem);

            return newItemDto;
        }

        private async Task UpdateTotalCost(UserBasket basket, BasketItem newItem, CancellationToken cancellationToken)
        {
            basket.TotalPrice += newItem.SumPrice;

            await unitOfWork.Basket
                .UpdateAsync(u => u.UserId.Equals(basket.UserId), basket, cancellationToken);
        }

        private async Task FindInBasketAsync(CreateBasketItemCommand comand, CancellationToken cancellationToken)
        {
            var itemInBasket = await unitOfWork.BasketItem
                .GetByConditionAsync(bi => bi.ItemId.Equals(comand.CreateBasketItemDto.ItemId)
                && bi.UserId.Equals(comand.UserId), cancellationToken);

            if (itemInBasket != null)
            {
                throw new AlreadyExistsException(BasketItemMessages.Exists);
            }
        }
    }
}