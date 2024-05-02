using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repositories;
using Catalogs.Domain.RequestFeatures;
using Catalogs.Infrastructure.Context;
using Catalogs.Infrastructure.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Repositories
{
    public sealed class ItemRepository(CatalogContext context) : Repository<Item>(context), IItemRepository
    {
        public async Task<PagedList<Item>> GetAllItemsOfTypeAsync(int typeId, ItemParameters itemParameters, bool trackChanges, CancellationToken token)
        {
            var items = await GetByCondition(i => i.TypeId.Equals(typeId), trackChanges)
                    .FilterItems(itemParameters)
                    .Search(itemParameters.SearchTerm)
                    .ToListAsync(token);

            return PagedList<Item>
                            .ToPagedList(items, itemParameters.PageNumber, itemParameters.PageSize);
        }

        public async Task<Item?> GetItemOfTypeByIdAsync(int typeId, int id, bool trackChanges, CancellationToken token) =>
            await GetByCondition(i => i.Id.Equals(id) && i.TypeId.Equals(typeId), trackChanges)
                .SingleOrDefaultAsync(token);

        public async Task<Item?> GetItemByIdAsync(int id, CancellationToken token)
        {
            var item = await GetByCondition(i => i.Id.Equals(id), trackChanges: false)
              .SingleOrDefaultAsync(token);

            return item;
        }
        public void AddItem(int typeId, Item item)
        {
            item.TypeId = typeId;
            Add(item);
        }
    }
}
