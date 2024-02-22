using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repos;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Repos
{
    public sealed class VendorRepository(CatalogContext context) : Repository<Vendor>(context), IVendorRepository
    {
        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync(bool trackChanges, CancellationToken token) =>
            await GetAll(trackChanges)
            .ToListAsync(token);

        public async Task<Vendor?> GetVendorByIdAsync(int id, bool trackChanges, CancellationToken token) =>
            await GetByCondition(v => v.Id.Equals(id), trackChanges)
                .Include(v => v.Items)
                .SingleOrDefaultAsync(token);

        public void DeleteVendor(Vendor vendor) => Delete(vendor);
    }
}
