using System.Linq.Expressions;

namespace Baskets.DataAccess.Repositories.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAllByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken);
        Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken);

        void Add(TEntity entity);
        void Update(Expression<Func<TEntity, bool>> condition, TEntity entity);
        Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken);
    }
}
