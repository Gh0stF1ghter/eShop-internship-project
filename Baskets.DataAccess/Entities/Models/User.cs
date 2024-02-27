using MongoDB.Bson;

using MongoDB.Bson.Serialization.Attributes;

namespace Baskets.DataAccess.Entities.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string BasketId { get; set; } = null!;

        [BsonIgnore]
        public BasketCustomer BasketCustomer { get; set; } = null!;
    }
}
