using Baskets.DataAccess.DBContext;
using Baskets.DataAccess.Repositories.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Baskets.DataAccess.Repositories.Implementations
{
    public class Repository<TEntity>(IMongoDbContext context, string collectionName) : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection = context.GetCollection<TEntity>(collectionName);

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken) =>
            await _collection.Find(t => true)
                       .ToListAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> GetAllByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken) =>
            await _collection.Find(condition)
                       .ToListAsync(cancellationToken);

        public async Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken) =>
            await _collection.Find(condition)
                       .SingleOrDefaultAsync(cancellationToken);

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken) =>
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

        public async Task UpdateAsync(Expression<Func<TEntity, bool>> condition, TEntity entity, CancellationToken cancellationToken) =>
            await _collection.ReplaceOneAsync(condition, entity, cancellationToken: cancellationToken);

        public async Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken) =>
            await _collection.FindOneAndDeleteAsync(condition, cancellationToken: cancellationToken);
    }
}