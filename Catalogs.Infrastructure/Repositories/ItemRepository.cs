using Catalogs.Domain.RequestFeatures;
using Catalogs.Infrastructure.Repositories.Extensions;

namespace Catalogs.Infrastructure.Repositories
{
    public sealed class ItemRepository(CatalogContext context, IDistributedCache distributedCache) : Repository<Item>(context), IItemRepository
    {
        public const string _cacheKeyBase = "ItemOfIds";
        public async Task<PagedList<Item>> GetAllItemsOfTypeAsync(int typeId, ItemParameters itemParameters, bool trackChanges, CancellationToken token)
        {
            var items = await GetByCondition(i => i.TypeId.Equals(typeId), trackChanges)
                     .FilterItems(itemParameters)
                     .Search(itemParameters.SearchTerm)
                     .ToListAsync(token);

            return PagedList<Item>
                            .ToPagedList(items, itemParameters.PageNumber, itemParameters.PageSize);
        }

        public async Task<Item?> GetItemOfTypeByIdAsync(int typeId, int id, bool trackChanges, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + typeId + id + trackChanges;

            var itemCache = await distributedCache.GetAsync(cacheKey, token);

            var item = new Item();

            if (itemCache is not null)
            {
                item = Cache<Item>.GetDataFromCache(itemCache);
            }
            else
            {
                item = await GetByCondition(i => i.Id.Equals(id) && i.TypeId.Equals(typeId), trackChanges)
                    .SingleOrDefaultAsync(token);

                itemCache = Cache<Item>.ConvertDataForCaching(item, out var options);

                await distributedCache.SetAsync(cacheKey, itemCache, options, token);
            }

            return item;
        }

        public async Task<Item> GetItemOfTypeToUpdateAsync(int typeId, int id, CancellationToken token)
        {
            var item = await GetItemOfTypeByIdAsync(typeId, id, trackChanges: true, token);

            var cacheKey = _cacheKeyBase + typeId + id;

            await RemoveAllCacheAsync(cacheKey, token);

            return item;
        }

        public void AddItem(int typeId, Item item)
        {
            item.TypeId = typeId;
            Add(item);
        }

        public async Task DeleteItemAsync(Item item, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + item.TypeId + item.Id;

            await RemoveAllCacheAsync(cacheKey, token);

            Delete(item);
        }

        private async Task RemoveAllCacheAsync(string objectCacheKey, CancellationToken token)
        {
            await distributedCache.RemoveAsync(objectCacheKey + "True", token);
            await distributedCache.RemoveAsync(objectCacheKey + "False", token);
        }
    }
}