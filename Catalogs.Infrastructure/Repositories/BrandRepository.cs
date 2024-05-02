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

            if (brandsCache is not null)
            {
                brands = Cache<List<Brand>>.GetDataFromCache(brandsCache);
            }
            else
            {
                brands = await GetAll(trackChanges)
                    .OrderBy(b => b.Name)
                    .ToListAsync(token);

                brandsCache = Cache<List<Brand>>.ConvertDataForCaching(brands, out var options);

                await distributedCache.SetAsync(_allCacheKey, brandsCache, options, token);
            }

            return brands;
        }

        public async Task<Brand?> GetBrandByIdAsync(int id, bool trackChanges, CancellationToken token)
        {
            var cacheKey = _cacheKeyBase + id + trackChanges;

            var brandCache = await distributedCache.GetAsync(cacheKey, token);

            var brand = new Brand();

            if (brandCache is not null)
            {
                brand = Cache<Brand>.GetDataFromCache(brandCache);
            }
            else
            {
                brand = await GetByCondition(b => b.Id.Equals(id), trackChanges)
                    .SingleOrDefaultAsync(token);

                brandCache = Cache<Brand>.ConvertDataForCaching(brand, out var options);

                await distributedCache.SetAsync(cacheKey, brandCache, options, token);
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