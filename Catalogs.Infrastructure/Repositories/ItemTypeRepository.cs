﻿using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repositories;
using Catalogs.Domain.RequestFeatures;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System;

namespace Catalogs.Infrastructure.Repositories
{
    public sealed class ItemTypeRepository(CatalogContext context, IDistributedCache distributedCache) : Repository<ItemType>(context), IItemTypeRepository
    {
        public async Task<IEnumerable<ItemType>> GetAllItemTypesAsync(bool trackChanges, CancellationToken token)
        {
            var cacheKey = $"AllTypes";

            var itemsCache = await distributedCache.GetAsync(cacheKey, token);

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

                await distributedCache.SetAsync(cacheKey, itemsCache, cachingOptions, token);
            }

            return itemTypes;
        }

        public async Task<ItemType?> GetItemTypeByIdAsync(int id, bool trackChanges, CancellationToken token) 
        {
            var cacheKey = $"Type{id}";

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
    }
}