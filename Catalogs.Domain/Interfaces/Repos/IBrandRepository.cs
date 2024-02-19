using Catalogs.Domain.Entities.Models;
using System.Linq.Expressions;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<IQueryable<Brand>> GetAllBrands(bool trackChanges);

        Task<Brand> GetBrandById(int id, bool trackChanges);

        Task<Vendor> GetBrandByConditionAsync(Expression<Func<Brand, bool>> condition, bool trackChanges);
    }
}
