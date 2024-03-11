namespace Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.UpdateBasketItemComand
{
    public record UpdateBasketItemComand(string UserId, string BasketItemId, int Quantity) : IRequest;
}