using AutoMapper;
using Baskets.BusinessLogic.Comands.BasketItem;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class CreateBasketItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateBasketItemComand, BasketItemDto>
    {
        public async Task<BasketItemDto> Handle(CreateBasketItemComand comand, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.Item.GetByConditionAsync(i => i.Id.Equals(comand.CreateBasketItemDto.ItemId), cancellationToken);

            await FindReferences(comand, item, cancellationToken);

            var newItem = mapper.Map<BasketItem>(comand.CreateBasketItemDto);

            newItem.UserId = comand.UserId;
            newItem.SumPrice = item.Price;

            unitOfWork.BasketItem.Add(newItem);

            var newItemDto = mapper.Map<BasketItemDto>(newItem);

            return newItemDto;
        }

        private async Task FindReferences(CreateBasketItemComand comand, Item item, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.User.GetByConditionAsync(u => u.Id.Equals(comand.UserId), cancellationToken);

            if (user == null)
            {
                throw new BadRequestException(UserMessages.NotFound);
            }

            var itemInBasket = await unitOfWork.BasketItem.GetByConditionAsync(bi => bi.ItemId.Equals(comand.CreateBasketItemDto.ItemId), cancellationToken);

            if (itemInBasket != null)
            {
                throw new BadRequestException(BasketItemMessages.Exists);
            }

            if (item == null)
            {
                throw new BadRequestException(ItemMessages.NotFound);
            }
        }
    }
}
