namespace Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.DeleteBasketItem
{
    public record DeleteBasketItemCommand(string UserId, string ItemId) : IRequest;
}