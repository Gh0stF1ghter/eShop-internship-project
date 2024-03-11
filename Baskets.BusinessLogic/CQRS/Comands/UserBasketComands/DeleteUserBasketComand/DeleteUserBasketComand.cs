namespace Baskets.BusinessLogic.CQRS.Comands.UserBasketComands.DeleteUserBasketComand
{
    public record DeleteUserBasketComand(string UserId) : IRequest;
}