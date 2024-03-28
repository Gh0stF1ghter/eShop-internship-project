namespace Baskets.BusinessLogic.DataTransferObjects
{
    public record BasketItemDto(string BasketItemId, int Quantity, double SumPrice, string ItemId, ItemDto Item);
}