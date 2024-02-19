namespace Catalogs.Domain.Entities.Models
{
    public class Vendor
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
