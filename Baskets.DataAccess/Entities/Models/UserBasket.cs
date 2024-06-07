using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Baskets.DataAccess.Entities.Models
{
    public class UserBasket
    {
        public double TotalPrice { get; set; } = 0;

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string UserId { get; set; } = null!;
    }
}
