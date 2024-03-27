namespace Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.DeleteUserBasket
{
    public record DeleteUserBasketCommand(string UserId) : IRequest;
}