using System.Linq.Expressions;

namespace Baskets.DataAccess.Repositories.Interfaces
{
    public interface IRepository<T> 
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> GetByConditionAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
