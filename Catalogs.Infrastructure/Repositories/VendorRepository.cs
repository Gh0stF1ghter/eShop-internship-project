using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repositories;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System;

namespace Catalogs.Infrastructure.Repositories
{
    public sealed class VendorRepository(CatalogContext context, IDistributedCache distributedCache) : Repository<Vendor>(context), IVendorRepository
    {
        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync(bool trackChanges, CancellationToken token)
        {
            var cacheKey = $"AllVendors";

            var itemCache = await distributedCache.GetAsync(cacheKey, token);

            var item = new List<Vendor>();

            if (itemCache != null)
            {
                var serializedItems = Encoding.UTF8.GetString(itemCache);
                item = JsonConvert.DeserializeObject<List<Vendor>>(serializedItems);
            }
            else
            {
                item = await GetAll(trackChanges)
                        .ToListAsync(token);

                var serializedItem = JsonConvert.SerializeObject(item);

                itemCache = Encoding.UTF8.GetBytes(serializedItem);

                var cachingOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));

                await distributedCache.SetAsync(cacheKey, itemCache, cachingOptions, token);
            }

            return item;

        }

        public async Task<Vendor?> GetVendorByIdAsync(int id, bool trackChanges, CancellationToken token)
        {
            var cacheKey = $"Vendor{id}";

            var itemCache = await distributedCache.GetAsync(cacheKey, token);

            var item = new Vendor();

            if (itemCache != null)
            {
                var serializedItems = Encoding.UTF8.GetString(itemCache);
                item = JsonConvert.DeserializeObject<Vendor>(serializedItems);
            }
            else
            {
                item = await GetByCondition(v => v.Id.Equals(id), trackChanges)
                .Include(v => v.Items)
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

        public void DeleteVendor(Vendor vendor) => Delete(vendor);
    }
}
