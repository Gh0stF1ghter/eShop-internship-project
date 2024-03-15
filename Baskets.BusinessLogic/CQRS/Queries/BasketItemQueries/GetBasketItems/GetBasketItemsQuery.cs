namespace Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItems
{
    public record GetBasketItemsQuery(string UserId) : IRequest<IEnumerable<BasketItemDto>>;
}
