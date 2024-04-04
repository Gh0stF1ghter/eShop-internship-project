using Baskets.DataAccess.Repositories.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Baskets.DataAccess.Repositories.Implementations
{
    public class Repository<TEntity>(IMongoCollection<TEntity> collection) : IRepository<TEntity> where TEntity : class
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken) =>
            await collection.Find(t => true)
                       .ToListAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> GetAllByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken) =>
            await collection.Find(condition)
                       .ToListAsync(cancellationToken);

        public async Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken) =>
            await collection.Find(condition)
                       .SingleOrDefaultAsync(cancellationToken);

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken) =>
            await collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

        public async Task UpdateAsync(Expression<Func<TEntity, bool>> condition, TEntity entity, CancellationToken cancellationToken) =>
            await collection.ReplaceOneAsync(condition, entity, cancellationToken: cancellationToken);

        public async Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken) =>
            await collection.FindOneAndDeleteAsync(condition, cancellationToken: cancellationToken);
    }
}