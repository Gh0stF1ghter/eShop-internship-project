namespace Catalogs.Domain.Entities.Models
{
    public class ItemType
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
