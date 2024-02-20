namespace Catalogs.Domain.Entities.DataTransferObjects
{
    public record ItemDto(int Id, string Name, int Stock, decimal Price, string? ImageUrl);
}
