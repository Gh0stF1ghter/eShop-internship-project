using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Baskets.DataAccess.Entities.Models
{
    public class UserBasket
    {
        public double TotalPrice { get; set; } = 0;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;
        [BsonIgnore]
        public User User { get; set; } = null!;
    }
}
