namespace Catalogs.Domain.Entities.Models
{
    public class Item
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public int TypeId { get; set; }
        public ItemType Type { get; set; } = null!;

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;
    }
}
