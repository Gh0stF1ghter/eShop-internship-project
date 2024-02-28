using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Baskets.DataAccess.Entities.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public int UserId { get; set; }
    }
}
