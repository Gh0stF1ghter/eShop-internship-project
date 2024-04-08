namespace Baskets.BusinessLogic.DataTransferObjects
{
    public record ItemDto(int Id, int BrandId, int VendorId, int TypeId, string Name, double Price, string ImageUrl);
}