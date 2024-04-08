using AutoMapper;
using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItems
{
    public class GetBasketItemsHandler(IUnitOfWork unitOfWork, ItemGrpcService.ItemService.ItemServiceClient client, IMapper mapper)
        : IRequestHandler<GetBasketItemsQuery, IEnumerable<BasketItemDto>>
    {
        public async Task<IEnumerable<BasketItemDto>> Handle(GetBasketItemsQuery query, CancellationToken cancellationToken)
        {
            await FindBasketAsync(query, cancellationToken);

            var items = await unitOfWork.BasketItem
                .GetAllBasketItemsAsync(query.UserId, cancellationToken);

            foreach (var basketItem in items)
            {
                var item = await FindCatalogItemAsync(basketItem.ItemId, cancellationToken);

                basketItem.Item = item;
            }

            var itemDtos = mapper.Map<IEnumerable<BasketItemDto>>(items);

            return itemDtos;
        }

        private async Task FindBasketAsync(GetBasketItemsQuery query, CancellationToken cancellationToken)
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