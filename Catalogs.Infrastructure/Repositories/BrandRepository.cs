using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repositories;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Repositories
{
    public sealed class BrandRepository(CatalogContext context) : Repository<Brand>(context), IBrandRepository
    {
        public async Task<IEnumerable<Brand>> GetAllBrandsAsync(bool trackChanges, CancellationToken token) =>
            await GetAll(trackChanges)
                    .OrderBy(b => b.Name)
                    .ToListAsync(token);

        public async Task<Brand?> GetBrandByIdAsync(int id, bool trackChanges, CancellationToken token) =>
            await GetByCondition(b => b.Id.Equals(id), trackChanges)
                .Include(b => b.Items)
                .SingleOrDefaultAsync(token);
    }
}
