namespace Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItemsQuery
{
    public record GetBasketItemsQuery(string UserId) : IRequest<IEnumerable<BasketItemDto>>;
}
