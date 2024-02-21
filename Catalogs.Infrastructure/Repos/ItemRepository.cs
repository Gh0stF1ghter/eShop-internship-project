using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repos;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Repos
{
    public sealed class ItemRepository(CatalogContext context) : Repository<Item>(context), IItemRepository
    {
        public async Task<IEnumerable<Item>> GetAllItemsAsync(bool trackChanges) =>
            await GetAll(trackChanges)
                    .ToListAsync();

        public async Task<IEnumerable<Item>> GetAllItemsOfTypeAsync(int typeId, bool trackChanges) =>
            await GetByCondition(i => i.TypeId.Equals(typeId), trackChanges).ToListAsync();

        public async Task<Item?> GetItemOfTypeByIdAsync(int typeid, int id, bool trackChanges) =>
            await GetByCondition(i => i.Id.Equals(id) && i.TypeId.Equals(typeid), trackChanges).SingleOrDefaultAsync();

        public void AddItem(int typeId, Item item)
        {
            item.TypeId = typeId;

            Add(item);
        }
    }
}
