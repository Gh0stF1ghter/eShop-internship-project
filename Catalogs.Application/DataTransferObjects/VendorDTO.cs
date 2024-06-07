namespace Catalogs.Application.DataTransferObjects
{
    public record VendorDto(int Id, string Name, List<ItemDto> Items);
}
