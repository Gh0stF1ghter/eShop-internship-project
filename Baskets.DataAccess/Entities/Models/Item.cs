using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Baskets.DataAccess.Entities.Models
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public int ItemId { get; set; }
        public required string Name { get; set; }
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
