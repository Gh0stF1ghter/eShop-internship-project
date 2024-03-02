namespace Baskets.BusinessLogic.Comands.BasketItem
{
    public record UpdateBasketItemComand(string UserId, string BasketItemId, int Quantity) : IRequest;
}