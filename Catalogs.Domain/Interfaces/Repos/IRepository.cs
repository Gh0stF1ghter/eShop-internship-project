using System.Linq.Expressions;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IRepository<T>
    {
        Task<IQueryable<T>> GetAllAsync(bool trackChanges);
        Task<IQueryable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate, bool trackChanges);
        Task<T?> GetByIdAsync(Guid id);
        
        Task CreateAsync(T entity);
        Task UpdateAsync(T oldEntity, T newEntity);
        void Delete(T entity);
    }
}
