namespace Catalogs.Domain.Entities.Models
{
    public class Brand
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
