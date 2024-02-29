namespace Baskets.BusinessLogic.Comands.BasketItem
{
    public record DeleteBasketItemComand(string BasketId, string ItemId) : IRequest;
}
