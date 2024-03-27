namespace Catalogs.Domain.Entities.Models
{
    public class Vendor
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
