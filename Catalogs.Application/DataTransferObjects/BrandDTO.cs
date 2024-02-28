namespace Catalogs.Application.DataTransferObjects
{
    public record BrandDto(int Id, string Name, List<ItemDto> Items);
}
