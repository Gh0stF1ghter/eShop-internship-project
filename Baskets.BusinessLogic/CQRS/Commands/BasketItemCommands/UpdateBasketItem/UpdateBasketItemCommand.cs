namespace Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.UpdateBasketItem
{
    public record UpdateBasketItemCommand(string UserId, string BasketItemId, int Quantity) : IRequest;
}