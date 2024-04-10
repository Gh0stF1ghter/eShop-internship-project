using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repositories;
using Catalogs.Domain.RequestFeatures;
using Catalogs.Infrastructure.Context;
using Catalogs.Infrastructure.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Catalogs.Infrastructure.Repositories
{
    public sealed class ItemRepository(CatalogContext context, IDistributedCache distributedCache) : Repository<Item>(context), IItemRepository
    {
        public async Task<PagedList<Item>> GetAllItemsOfTypeAsync(int typeId, ItemParameters itemParameters, bool trackChanges, CancellationToken token)
        {
            var cacheKey = $"ItemsOfType{typeId}andParams{itemParameters}";

            var itemsCache = await distributedCache.GetAsync(cacheKey, token);

            var items = new List<Item>();

            if (itemsCache != null)
            {
                var serializedItems = Encoding.UTF8.GetString(itemsCache);
                items = JsonConvert.DeserializeObject<List<Item>>(serializedItems);
            }
            else
            {
                items = await GetByCondition(i => i.TypeId.Equals(typeId), trackChanges)
                     .FilterItems(itemParameters)
                     .Search(itemParameters.SearchTerm)
                     .ToListAsync(token);

                var serializedItems = JsonConvert.SerializeObject(items);

                itemsCache = Encoding.UTF8.GetBytes(serializedItems);

                var cachingOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));

                await distributedCache.SetAsync(cacheKey, itemsCache, cachingOptions, token);
            }

            return PagedList<Item>
                            .ToPagedList(items, itemParameters.PageNumber, itemParameters.PageSize);
        }

        public async Task<Item?> GetItemOfTypeByIdAsync(int typeId, int id, bool trackChanges, CancellationToken token)
        {
            var cacheKey = $"ItemOfType{typeId}AndId{id}";

            var itemCache = await distributedCache.GetAsync(cacheKey, token);

            var item = new Item();

            if (itemCache != null)
            {
                var serializedItems = Encoding.UTF8.GetString(itemCache);
                item = JsonConvert.DeserializeObject<Item>(serializedItems);
            }
            else
            {
                item = await GetByCondition(i => i.Id.Equals(id) && i.TypeId.Equals(typeId), trackChanges)
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

        public void AddItem(int typeId, Item item)
        {
            item.TypeId = typeId;
            Add(item);
        }
    }
}