namespace Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.CreateUserBasket
{
    public record CreateUserBasketCommand(string UserId) : IRequest<UserBasketDto>;
}