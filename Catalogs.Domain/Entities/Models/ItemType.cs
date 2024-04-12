namespace Catalogs.Domain.Entities.Models
{
    public class ItemType
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<Item>? Items { get; set; }
    }
}
