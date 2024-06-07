namespace Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItem
{
    public record GetBasketItemQuery(string UserId, string BasketItemId) : IRequest<BasketItemDto>;
}
