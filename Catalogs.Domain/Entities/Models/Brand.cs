namespace Catalogs.Domain.Entities.Models
{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<Item>? Items { get; set; }
    }
}
