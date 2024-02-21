using System.Linq.Expressions;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(bool trackChanges);
        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate, bool trackChanges);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
