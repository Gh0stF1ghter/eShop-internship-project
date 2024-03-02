namespace Baskets.BusinessLogic.Queries.BasketItem
{
    public record GetBasketItemQuery(string UserId, string BasketItemId) : IRequest<BasketItemDto>;
}
