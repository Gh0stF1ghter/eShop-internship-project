namespace Baskets.BusinessLogic.Queries.BasketItem
{
    public record GetBasketItemsQuery(string BasketId) : IRequest<IEnumerable<BasketItemDto>>;
}
