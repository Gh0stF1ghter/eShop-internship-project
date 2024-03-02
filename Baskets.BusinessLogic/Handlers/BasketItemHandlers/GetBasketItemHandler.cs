using AutoMapper;
using Baskets.BusinessLogic.Queries.BasketItem;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class GetBasketItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBasketItemQuery, BasketItemDto>
    {
        public async Task<BasketItemDto> Handle(GetBasketItemQuery query, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket.GetByConditionAsync(b => b.UserId.Equals(query.UserId), cancellationToken);

            if (basket == null)
            {
                throw new BadRequestException(UserBasketMessages.NotFound);
            }

            var basketItem = await unitOfWork.BasketItem.GetBasketItemByConditionAsync(bi => bi.Id.Equals(query.BasketItemId), cancellationToken);

            if (basketItem == null)
            {
                throw new NotFoundException(BasketItemMessages.NotFound);
            }

            var basketItemDto = mapper.Map<BasketItemDto>(basketItem);

            return basketItemDto;
        }
    }
}
