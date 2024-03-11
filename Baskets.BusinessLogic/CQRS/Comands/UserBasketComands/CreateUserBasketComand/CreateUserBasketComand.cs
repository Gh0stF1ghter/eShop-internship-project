namespace Baskets.BusinessLogic.CQRS.Comands.UserBasketComands.CreateUserBasketComand
{
    public record CreateUserBasketComand(string UserId) : IRequest<UserBasketDto>;
}