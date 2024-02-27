using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.ObjectModel;

namespace Baskets.DataAccess.Entities.Models
{
    public class BasketCustomer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> BasketItemsIds { get; set; } = null!;

        [BsonIgnore]
        public List<BasketItem> BasketItemList { get; set; } = null!;
    }
}
