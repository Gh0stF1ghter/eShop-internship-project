using Catalogs.Domain.Entities.Models;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync(bool trackChanges, CancellationToken token);
        Task<Brand?> GetBrandByIdAsync(int id, bool trackChanges, CancellationToken token);

        void DeleteBrand(Brand brand);
    }
}
