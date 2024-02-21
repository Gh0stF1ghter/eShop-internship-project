namespace Catalogs.Domain.Entities.DataTransferObjects
{
    public record VendorDto(int Id, string Name, List<ItemDto> Items);
}
