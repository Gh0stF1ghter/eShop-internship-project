namespace Catalogs.Domain.Entities.Models
{
    public class Item
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        public Guid BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public Guid TypeId { get; set; }
        public Type Type { get; set; } = null!;

        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;
    }
}
