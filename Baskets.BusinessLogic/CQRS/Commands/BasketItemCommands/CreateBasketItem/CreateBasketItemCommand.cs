namespace Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.CreateBasketItem
{
    public record CreateBasketItemCommand(string UserId, CreateBasketItemDto CreateBasketItemDto) : IRequest<BasketItemDto>;
}