namespace Baskets.BusinessLogic.CQRS.Queries.UserBasketQueries.GetUserBasketQuery
{
    public record GetUserBasketQuery(string UserId) : IRequest<UserBasketDto>;
}
