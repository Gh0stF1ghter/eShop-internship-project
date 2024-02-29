using AutoMapper;
using Baskets.BusinessLogic.Queries.BasketItem;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class GetBasketItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBasketItemQuery, BasketItemDto>
    {
        public async Task<BasketItemDto> Handle(GetBasketItemQuery comand, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket.GetByConditionAsync(b => b.UserId.Equals(comand.UserId), cancellationToken);

            if (basket == null)
            {
                throw new BadRequestException(UserBasketMessages.NotFound);
            }

            var basketItem = await unitOfWork.BasketItem.GetByConditionAsync(bi => bi.Id.Equals(comand.ItemId), cancellationToken);

            if (basketItem == null)
            {
                throw new NotFoundException(BasketItemMessages.NotFound);
            }

            var basketItemDto = mapper.Map<BasketItemDto>(basket);

            return basketItemDto;
        }
    }
}
