using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repos;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Repos
{
    public sealed class ItemTypeRepository(CatalogContext context) : Repository<ItemType>(context), IItemTypeRepository
    {
        public async Task<IEnumerable<ItemType>> GetAllItemTypesAsync(bool trackChanges, CancellationToken token) =>
            await GetAll(trackChanges)
                .ToListAsync(token);

        public async Task<ItemType?> GetItemTypeByIdAsync(int id, bool trackChanges, CancellationToken token) =>
            await GetByCondition(t => t.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync(token);
    }
}
