using Amazon.Auth.AccessControlPolicy;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.DataAccess.Repositories.Implementations
{
    public class Repository<TEntity>(IMongoClient client, IOptions<BasketDatabaseSettings> options, string collectionName) : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection = client.GetDatabase(options.Value.DatabaseName)
                                                                       .GetCollection<TEntity>(collectionName);
        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken) =>
            await _collection.Find(t => true)
                       .ToListAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> GetAllByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken) =>
            await _collection.Find(condition).ToListAsync(cancellationToken);

        public async Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken) =>
            await _collection.Find(condition).SingleOrDefaultAsync(cancellationToken);

        public void Add(TEntity entity) =>
            _collection.InsertOne(entity);

        public void Update(Expression<Func<TEntity, bool>> condition, TEntity entity) =>
            _collection.ReplaceOne(condition, entity);

        public async Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken) =>
            await _collection.FindOneAndDeleteAsync(condition, cancellationToken: cancellationToken);
    }
}
