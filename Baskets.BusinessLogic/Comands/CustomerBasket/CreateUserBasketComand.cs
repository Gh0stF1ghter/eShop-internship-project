namespace Baskets.BusinessLogic.Comands.CustomerBasket
{
    public record CreateUserBasketComand(string UserId) : IRequest<UserBasketDto>;
}