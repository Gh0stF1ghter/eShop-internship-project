namespace Baskets.BusinessLogic.DataTransferObjects
{
    public record BasketItemDto(string BasketItemId, int Quantity, double SumPrice, ItemDto Item);
}