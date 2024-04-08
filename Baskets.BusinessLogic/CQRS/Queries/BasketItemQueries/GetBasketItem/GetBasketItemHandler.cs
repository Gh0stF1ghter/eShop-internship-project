using AutoMapper;
using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItem
{
    public class GetBasketItemHandler(IUnitOfWork unitOfWork,
        ItemGrpcService.ItemService.ItemServiceClient client,
        IMapper mapper)
        : IRequestHandler<GetBasketItemQuery, BasketItemDto>
    {
        public async Task<BasketItemDto> Handle(GetBasketItemQuery query, CancellationToken cancellationToken)
        {
            await FindBasket(query, cancellationToken);

            var basketItem = await unitOfWork.BasketItem
                .GetBasketItemByConditionAsync(bi => bi.BasketItemId.Equals(query.BasketItemId), cancellationToken);

            if (basketItem == null)
            {
                throw new NotFoundException(BasketItemMessages.NotFound);
            }

            basketItem.Item = await FindCatalogItemAsync(basketItem.ItemId, cancellationToken);

            var basketItemDto = mapper.Map<BasketItemDto>(basketItem);

            return basketItemDto;
        }

        private async Task FindBasket(GetBasketItemQuery query, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket
                .GetByConditionAsync(b => b.UserId.Equals(query.UserId), cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException(UserBasketMessages.NotFound);
            }
        }

        private async Task<Item> FindCatalogItemAsync(int itemId, CancellationToken cancellationToken = default)
        {
            var itemRequest = new ItemGrpcService.GetItemRequest { Id = itemId };

            var itemResponse = await client.GetItemAsync(itemRequest, cancellationToken: cancellationToken);

            var grpcItem = itemResponse.Item;

            if (grpcItem == null)
            {
                throw new NotFoundException(ItemMessages.NotFound);
            }

            var item = mapper.Map<Item>(grpcItem);

            return item;
        }
    }
}