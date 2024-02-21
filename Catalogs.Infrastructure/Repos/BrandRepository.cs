using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repos;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Repos
{
    public sealed class BrandRepository(CatalogContext context) : Repository<Brand>(context), IBrandRepository
    {
        public async Task<IEnumerable<Brand>> GetAllBrandsAsync(bool trackChanges, CancellationToken token) =>
            await GetAll(trackChanges)
                    .OrderBy(b => b.Name)
                    .Include(b => b.Items)
                    .ToListAsync(token);

        public async Task<Brand?> GetBrandByIdAsync(int id, bool trackChanges, CancellationToken token) =>
            await GetByCondition(b => b.Id.Equals(id), trackChanges)
                .Include(b => b.Items)
                .SingleOrDefaultAsync(token);

        public void DeleteBrand(Brand brand) => Delete(brand);
    }
}
