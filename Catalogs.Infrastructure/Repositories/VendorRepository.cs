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
                vendors = Cache<List<Vendor>>.GetDataFromCache(vendorsCache);
            }
            else
            {
                vendors = await GetAll(trackChanges)
                        .ToListAsync(token);

                vendorsCache = Cache<List<Vendor>>.ConvertDataForCaching(vendors, out var options);

                await distributedCache.SetAsync(_allCacheKey, vendorsCache, options, token);
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
                vendor = Cache<Vendor>.GetDataFromCache(vendorCache);
            }
            else
            {
                vendor = await GetByCondition(v => v.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync(token);

                vendorCache = Cache<Vendor>.ConvertDataForCaching(vendor, out var options);

                await distributedCache.SetAsync(cacheKey, vendorCache, options, token);
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