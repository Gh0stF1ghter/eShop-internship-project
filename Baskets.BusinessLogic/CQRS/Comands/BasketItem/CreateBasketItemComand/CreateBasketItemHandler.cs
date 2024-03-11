using AutoMapper;
using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.CreateBasketItemComand
{
    public class CreateBasketItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateBasketItemComand, BasketItemDto>
    {
        public async Task<BasketItemDto> Handle(CreateBasketItemComand comand, CancellationToken cancellationToken)
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

            unitOfWork.BasketItem.Add(newItem);

            UpdateTotalCost(basket, newItem);

            var newItemDto = mapper.Map<BasketItemDto>(newItem);

            return newItemDto;
        }

        private void UpdateTotalCost(UserBasket basket, BasketItem newItem)
        {
            basket.TotalPrice += newItem.SumPrice;

            unitOfWork.Basket
                .Update(u => u.UserId.Equals(basket.UserId), basket);
        }

        private async Task FindInBasketAsync(CreateBasketItemComand comand, CancellationToken cancellationToken)
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