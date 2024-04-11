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
        private const string _allCacheKey = "AllVendors";
        private const string _cacheKeyBase = "Vendor";

        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync(bool trackChanges, CancellationToken token)
        {
            var vendorsCache = await distributedCache.GetAsync(_allCacheKey, token);

            var vendors = new List<Vendor>();

            if (vendorsCache is not null)
            {
                var serializedVendors = Encoding.UTF8.GetString(vendorsCache);
                vendors = JsonConvert.DeserializeObject<List<Vendor>>(serializedVendors);
            }
            else
            {
                vendors = await GetAll(trackChanges)
                        .ToListAsync(token);

                var serializedVendors = JsonConvert.SerializeObject(vendors);

                vendorsCache = Encoding.UTF8.GetBytes(serializedVendors);

                var cachingOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));

                await distributedCache.SetAsync(_allCacheKey, vendorsCache, cachingOptions, token);
            }

            return vendors;

        }

        public async Task<Vendor?> GetVendorByIdAsync(int id, bool trackChanges, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + id + trackChanges;

            var vendorCache = await distributedCache.GetAsync(cacheKey, token);

            var vendor = new Vendor();

            if (vendorCache is not null)
            {
                var serializedItems = Encoding.UTF8.GetString(vendorCache);
                vendor = JsonConvert.DeserializeObject<Vendor>(serializedItems);
            }
            else
            {
                vendor = await GetByCondition(v => v.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync(token);

                var serializedVendor = JsonConvert.SerializeObject(vendor);

                vendorCache = Encoding.UTF8.GetBytes(serializedVendor);

                var cachingOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));

                await distributedCache.SetAsync(cacheKey, vendorCache, cachingOptions, token);
            }

            return vendor;
        }

        public async Task<Vendor> GetVendorToUpdateAsync(int id, CancellationToken token)
        {
            var vendor = await GetVendorByIdAsync(id, trackChanges: true, token);

            var cacheKey = _cacheKeyBase + id;

            await RemoveAllCacheAsync(cacheKey, token);

            return vendor;
        }

        public async Task AddVendorAsync(Vendor vendor, CancellationToken token)
        {
            await distributedCache.RemoveAsync(_allCacheKey, token);

            Add(vendor);
        }

        public async Task DeleteVendorAsync(Vendor vendor, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + vendor.Id;

            await RemoveAllCacheAsync(cacheKey, token);

            Delete(vendor);
        }

        private async Task RemoveAllCacheAsync(string objectCacheKey, CancellationToken token)
        {
            await distributedCache.RemoveAsync(_allCacheKey, token);
            await distributedCache.RemoveAsync(objectCacheKey + "True", token);
            await distributedCache.RemoveAsync(objectCacheKey + "False", token);
        }
    }
}