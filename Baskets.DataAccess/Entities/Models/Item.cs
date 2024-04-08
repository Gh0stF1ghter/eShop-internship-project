using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Baskets.DataAccess.Entities.Models
{
    public class Item
    {
        public int Id { get; set; }

        public int BrandId { get; set; }
        public int VendorId { get; set; }
        public int TypeId { get; set; }

        public required string Name { get; set; }
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
