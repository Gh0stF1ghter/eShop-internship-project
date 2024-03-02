namespace Baskets.BusinessLogic.Queries.CustomerBasket
{
    public record GetUserBasketQuery(string UserId) : IRequest<UserBasketDto>;
}
