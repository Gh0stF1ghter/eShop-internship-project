using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Baskets.DataAccess.Entities.Models
{
    public class CustomerBasket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public int TotalPrice { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;
        [BsonIgnore]
        public User User { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> BasketItemsIds { get; set; } = null!;
        [BsonIgnore]
        public List<BasketItem> BasketItemList { get; set; } = null!;
    }
}
