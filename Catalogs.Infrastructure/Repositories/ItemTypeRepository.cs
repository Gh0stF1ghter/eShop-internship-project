namespace Catalogs.Infrastructure.Repositories
{
    public sealed class ItemTypeRepository(CatalogContext context, IDistributedCache distributedCache) : Repository<ItemType>(context), IItemTypeRepository
    {
        private const string _allCacheKey = "AllTypes";
        private const string _cacheKeyBase = "Type";

        public async Task<IEnumerable<ItemType>> GetAllItemTypesAsync(bool trackChanges, CancellationToken token)
        {
            var itemTypesCache = await distributedCache.GetAsync(_allCacheKey, token);

            var itemTypes = new List<ItemType>();

            if (itemTypesCache is not null)
            {
                itemTypes = Cache<List<ItemType>>.GetDataFromCache(itemTypesCache);
            }
            else
            {
                itemTypes = await GetAll(trackChanges)
                    .ToListAsync(token);

                itemTypesCache = Cache<List<ItemType>>.ConvertDataForCaching(itemTypes, out var options);

                await distributedCache.SetAsync(_allCacheKey, itemTypesCache, options, token);
            }

            return itemTypes;
        }

        public async Task<ItemType?> GetItemTypeByIdAsync(int id, bool trackChanges, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + id + trackChanges;

            var itemTypeCache = await distributedCache.GetAsync(cacheKey, token);

            var itemType = new ItemType();

            if (itemTypeCache != null)
            {
                itemType = Cache<ItemType>.GetDataFromCache(itemTypeCache);
            }
            else
            {
                itemType = await GetByCondition(t => t.Id.Equals(id), trackChanges)
                    .SingleOrDefaultAsync(token);

                itemTypeCache = Cache<ItemType>.ConvertDataForCaching(itemType, out var options);

                await distributedCache.SetAsync(cacheKey, itemTypeCache, options, token);
            }

            return itemType;
        }

        public async Task<ItemType> GetItemTypeToUpdateAsync(int id, CancellationToken token)
        {
            var itemType = await GetItemTypeByIdAsync(id, trackChanges: true, token);

            var cacheKey = _cacheKeyBase + id;

            await RemoveAllCacheAsync(cacheKey, token);

            return itemType;
        }

        public async Task AddItemTypeAsync(ItemType itemType, CancellationToken token)
        {
            await distributedCache.RemoveAsync(_allCacheKey, token);

            Add(itemType);
        }

        public async Task DeleteItemTypeAsync(ItemType itemType, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + itemType.Id;

            await RemoveAllCacheAsync(cacheKey, token);

            Delete(itemType);
        }

        private async Task RemoveAllCacheAsync(string objectCacheKey, CancellationToken token)
        {
            await distributedCache.RemoveAsync(_allCacheKey, token);
            await distributedCache.RemoveAsync(objectCacheKey + "True", token);
            await distributedCache.RemoveAsync(objectCacheKey + "False", token);
        }
    }
}