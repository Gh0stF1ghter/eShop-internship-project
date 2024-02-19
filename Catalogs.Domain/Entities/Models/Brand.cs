namespace Catalogs.Domain.Entities.Models
{
    public class Brand
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
