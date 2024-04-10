using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repositories;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Catalogs.Infrastructure.Repositories
{
    public sealed class BrandRepository(CatalogContext context, IDistributedCache distributedCache) : Repository<Brand>(context), IBrandRepository
    {
        public async Task<IEnumerable<Brand>> GetAllBrandsAsync(bool trackChanges, CancellationToken token) 
        {
            var cacheKey = $"AllBrands";

            var itemsCache = await distributedCache.GetAsync(cacheKey, token);

            var itemTypes = new List<Brand>();

            if (itemsCache != null)
            {
                var serializedItems = Encoding.UTF8.GetString(itemsCache);
                itemTypes = JsonConvert.DeserializeObject<List<Brand>>(serializedItems);
            }
            else
            {
                itemTypes = await GetAll(trackChanges)
                    .OrderBy(b => b.Name)
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

        public async Task<Brand?> GetBrandByIdAsync(int id, bool trackChanges, CancellationToken token)
        {
            var cacheKey = $"Brand{id}";

            var itemCache = await distributedCache.GetAsync(cacheKey, token);

            var item = new Brand();

            if (itemCache != null)
            {
                var serializedItems = Encoding.UTF8.GetString(itemCache);
                item = JsonConvert.DeserializeObject<Brand>(serializedItems);
            }
            else
            {
                item = await GetByCondition(b => b.Id.Equals(id), trackChanges)
                    .Include(b => b.Items)
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
