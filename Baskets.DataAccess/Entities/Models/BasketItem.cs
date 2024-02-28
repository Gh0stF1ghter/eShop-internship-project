using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Baskets.DataAccess.Entities.Models
{
    public class BasketItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int Quantity { get; set; }
        public double SumPrice { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ItemId { get; set; }

        [BsonIgnore]
        public Item Item { get; set; } = null!;
    }
}
