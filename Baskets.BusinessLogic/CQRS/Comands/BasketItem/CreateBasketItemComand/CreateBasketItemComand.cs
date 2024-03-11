namespace Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.CreateBasketItemComand
{
    public record CreateBasketItemComand(string UserId, CreateBasketItemDto CreateBasketItemDto) : IRequest<BasketItemDto>;
}