using Baskets.DataAccess.Entities.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.DataAccess.DBContext
{
    public class MongoDbContext(IOptions<BasketDatabaseSettings> options, IMongoClient client) : IMongoDbContext
    {
        private readonly IMongoDatabase _db = client.GetDatabase(options.Value.DatabaseName);

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
           new SeedTestData(_db, options).Seed();

           return _db.GetCollection<T>(collectionName);
        }
    }
}
