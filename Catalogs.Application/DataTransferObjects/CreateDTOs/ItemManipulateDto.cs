namespace Catalogs.Application.DataTransferObjects.CreateDTOs
{
    public record ItemManipulateDto(string Name, int Stock, decimal Price, string ImageUrl, int VendorId, int BrandId);
}
