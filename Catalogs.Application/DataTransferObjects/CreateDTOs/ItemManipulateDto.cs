namespace Catalogs.Application.DataTransferObjects.CreateDTOs
{
    public record ItemManipulateDto(string Name, int Stock, double Price, string ImageUrl, int VendorId, int BrandId);
}
