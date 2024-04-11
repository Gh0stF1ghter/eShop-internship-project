using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repositories;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Catalogs.Infrastructure.Repositories
{
    public sealed class ItemTypeRepository(CatalogContext context, IDistributedCache distributedCache) : Repository<ItemType>(context), IItemTypeRepository
    {
        private const string _allCacheKey = "AllTypes";
        private const string _cacheKeyBase = "Type";

        public async Task<IEnumerable<ItemType>> GetAllItemTypesAsync(bool trackChanges, CancellationToken token)
        {
            var itemsCache = await distributedCache.GetAsync(_allCacheKey, token);

            var itemTypes = new List<ItemType>();

            if (itemsCache != null)
            {
                var serializedItems = Encoding.UTF8.GetString(itemsCache);
                itemTypes = JsonConvert.DeserializeObject<List<ItemType>>(serializedItems);
            }
            else
            {
                itemTypes = await GetAll(trackChanges)
                    .ToListAsync(token);

                var serializedItems = JsonConvert.SerializeObject(itemTypes);

                itemsCache = Encoding.UTF8.GetBytes(serializedItems);

                var cachingOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));

                await distributedCache.SetAsync(_allCacheKey, itemsCache, cachingOptions, token);
            }

            return itemTypes;
        }

        public async Task<ItemType?> GetItemTypeByIdAsync(int id, bool trackChanges, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + id + trackChanges;

            var itemCache = await distributedCache.GetAsync(cacheKey, token);

            var item = new ItemType();

            if (itemCache != null)
            {
                var serializedItems = Encoding.UTF8.GetString(itemCache);
                item = JsonConvert.DeserializeObject<ItemType>(serializedItems);
            }
            else
            {
                item = await GetByCondition(t => t.Id.Equals(id), trackChanges)
                    .SingleOrDefaultAsync(token);

                var serializedItem = JsonConvert.SerializeObject(item);

                itemCache = Encoding.UTF8.GetBytes(serializedItem);

                var cachingOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));

                await distributedCache.SetAsync(cacheKey, itemCache, cachingOptions, token);
            }

            return item;
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