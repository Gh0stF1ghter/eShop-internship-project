namespace Baskets.BusinessLogic.Comands.BasketItem
{
    public record CreateBasketItemComand(string UserId, CreateBasketItemDto CreateBasketItemDto) : IRequest<BasketItemDto>;
}