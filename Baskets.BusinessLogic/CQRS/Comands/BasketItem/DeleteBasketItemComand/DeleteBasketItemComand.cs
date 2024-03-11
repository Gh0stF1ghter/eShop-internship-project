namespace Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.DeleteBasketItemComand
{
    public record DeleteBasketItemComand(string UserId, string ItemId) : IRequest;
}