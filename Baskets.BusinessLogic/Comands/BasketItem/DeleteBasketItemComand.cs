namespace Baskets.BusinessLogic.Comands.BasketItem
{
    public record DeleteBasketItemComand(string UserId, string ItemId) : IRequest;
}
