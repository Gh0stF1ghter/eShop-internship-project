using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repositories;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Catalogs.Infrastructure.Repositories
{
    public sealed class BrandRepository(CatalogContext context, IDistributedCache distributedCache) : Repository<Brand>(context), IBrandRepository
    {
        private const string _allCacheKey = "AllBrands";
        private const string _cacheKeyBase = "Brand";

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync(bool trackChanges, CancellationToken token) 
        {
            var brandsCache = await distributedCache.GetAsync(_allCacheKey, token);

            var brands = new List<Brand>();

            if (brandsCache != null)
            {
                var serializedBrands = Encoding.UTF8.GetString(brandsCache);
                brands = JsonConvert.DeserializeObject<List<Brand>>(serializedBrands);
            }
            else
            {
                brands = await GetAll(trackChanges)
                    .OrderBy(b => b.Name)
                    .ToListAsync(token);

                var serializedBrands = JsonConvert.SerializeObject(brands);

                brandsCache = Encoding.UTF8.GetBytes(serializedBrands);

                var cachingOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));

                await distributedCache.SetAsync(_allCacheKey, brandsCache, cachingOptions, token);
            }

            return brands;
        }

        public async Task<Brand?> GetBrandByIdAsync(int id, bool trackChanges, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + id + trackChanges;

            var brandCache = await distributedCache.GetAsync(cacheKey, token);

            var brand = new Brand();

            if (brandCache != null)
            {
                var serializedBrand = Encoding.UTF8.GetString(brandCache);
                brand = JsonConvert.DeserializeObject<Brand>(serializedBrand);
            }
            else
            {
                brand = await GetByCondition(b => b.Id.Equals(id), trackChanges)
                    .SingleOrDefaultAsync(token);

                var serializedBrand = JsonConvert.SerializeObject(brand);

                brandCache = Encoding.UTF8.GetBytes(serializedBrand);

                var cachingOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));

                await distributedCache.SetAsync(cacheKey, brandCache, cachingOptions, token);
            }

            return brand;
        }

        public async Task<Brand> GetBrandToUpdateAsync(int id, CancellationToken token)
        {
            var brand = await GetBrandByIdAsync(id, trackChanges: true, token);

            var cacheKey = _cacheKeyBase + id;

            await RemoveAllCacheAsync(cacheKey, token);

            return brand;
        }

        public async Task AddBrandAsync(Brand brand, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + brand.Id;

            await RemoveAllCacheAsync(cacheKey, token);

            Add(brand);
        }

        public async Task DeleteBrandAsync(Brand brand, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + brand.Id;

            await RemoveAllCacheAsync(cacheKey, token);

            Delete(brand);
        }

        private async Task RemoveAllCacheAsync(string objectCacheKey, CancellationToken token)
        {
            await distributedCache.RemoveAsync(_allCacheKey, token);
            await distributedCache.RemoveAsync(objectCacheKey + "True", token);
            await distributedCache.RemoveAsync(objectCacheKey + "False", token);
        }
    }
}