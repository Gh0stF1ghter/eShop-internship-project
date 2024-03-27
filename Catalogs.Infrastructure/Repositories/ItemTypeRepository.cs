using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repositories;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Repositories
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
