using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repos;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogs.Infrastructure.Repos
{
    public sealed class ItemRepository(CatalogContext context) : Repository<Item>(context), IItemRepository
    {
        public async Task<IEnumerable<Item>> GetAllItemsAsync(bool trackChanges) =>
            await GetAll(trackChanges)
                    .ToListAsync();

        public async Task<IEnumerable<Item>> GetAllItemsOfBrandAsync(int brandId, bool trackChanges) =>
            await GetByCondition(i => i.BrandId.Equals(brandId), trackChanges).ToListAsync();

        public async Task<IEnumerable<Item>> GetAllItemsOfTypeAsync(int typeId, bool trackChanges) =>
            await GetByCondition(i => i.TypeId.Equals(typeId), trackChanges).ToListAsync();

        public async Task<IEnumerable<Item>> GetAllItemsOfVendorAsync(int vendorId, bool trackChanges) =>
            await GetByCondition(i => i.VendorId.Equals(vendorId), trackChanges).ToListAsync();

        public async Task<Item?> GetItemByIdAsync(int id, bool trackChanges) =>
            await GetByCondition(i => i.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public void AddItem(int brandId, int typeId, int Vendor, Item item)
        {
            item.BrandId = brandId;
            item.TypeId = typeId;
            item.VendorId = Vendor;

            Add(item);
        }
    }
}
