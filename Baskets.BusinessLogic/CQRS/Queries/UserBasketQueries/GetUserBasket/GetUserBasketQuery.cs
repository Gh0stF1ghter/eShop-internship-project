namespace Baskets.BusinessLogic.CQRS.Queries.UserBasketQueries.GetUserBasket
{
    public record GetUserBasketQuery(string UserId) : IRequest<UserBasketDto>;
}
