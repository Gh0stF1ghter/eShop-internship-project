namespace Catalogs.Application.DataTransferObjects
{
    public record ItemDto(int Id, int BrandId, int VendorId, string Name, int Stock, decimal Price, string? ImageUrl);
}
