namespace Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs
{
    public record ItemManipulateDto(string Name, int Stock, decimal Price, string ImageUrl);
}
