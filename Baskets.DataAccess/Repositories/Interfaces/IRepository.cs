using System.Linq.Expressions;

namespace Baskets.DataAccess.Repositories.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAllByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken);
        Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task UpdateAsync(Expression<Func<TEntity, bool>> condition, TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken);
    }
}
