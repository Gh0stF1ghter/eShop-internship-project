﻿namespace Catalogs.Application.DataTransferObjects
{
    public record ItemDto(int Id, string Name, int Stock, decimal Price, string? ImageUrl, int BrandId, int VendorId, int TypeId);
}
