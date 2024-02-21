namespace Catalogs.Domain.Entities.DataTransferObjects
{
    public record BrandDto(int Id, string Name, List<ItemDto> Items);
}
