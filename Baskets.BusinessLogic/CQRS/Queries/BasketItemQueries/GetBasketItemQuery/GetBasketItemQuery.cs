namespace Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItemQuery
{
    public record GetBasketItemQuery(string UserId, string BasketItemId) : IRequest<BasketItemDto>;
}
