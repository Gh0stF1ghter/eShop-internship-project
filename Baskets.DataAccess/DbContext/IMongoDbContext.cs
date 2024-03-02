using MongoDB.Driver;

namespace Baskets.DataAccess.DBContext
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}
