using System.Linq.Expressions;

namespace Catalogs.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(bool trackChanges);
        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate, bool trackChanges);

        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
