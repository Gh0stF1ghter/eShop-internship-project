using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Baskets.DataAccess.Entities.Models
{
    public class BasketItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BasketItemId { get; set; } = null!;

        public int Quantity { get; set; } = 1;
        public double SumPrice { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;
        [BsonIgnore]
        public UserBasket CustomerBasket { get; set; } = null!;

        [BsonRepresentation(BsonType.Int32)]
        public int ItemId { get; set; }
        [BsonIgnore]
        public Item Item { get; set; } = null!;
    }
}