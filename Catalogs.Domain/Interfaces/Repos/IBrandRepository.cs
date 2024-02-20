using Catalogs.Domain.Entities.Models;
using System.Linq.Expressions;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync(bool trackChanges);
        Task<Brand?> GetBrandByIdAsync(int id, bool trackChanges);
    }
}
