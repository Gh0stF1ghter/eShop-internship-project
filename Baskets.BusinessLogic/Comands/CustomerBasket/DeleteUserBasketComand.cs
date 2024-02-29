using MediatR;

namespace Baskets.BusinessLogic.Comands.CustomerBasket
{
    public record DeleteUserBasketComand(string UserId) : IRequest;
}
