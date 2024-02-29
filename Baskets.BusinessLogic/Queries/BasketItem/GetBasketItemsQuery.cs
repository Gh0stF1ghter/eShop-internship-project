namespace Baskets.BusinessLogic.Queries.BasketItem
{
    public record GetBasketItemsQuery(string UserId) : IRequest<IEnumerable<BasketItemDto>>;
}
