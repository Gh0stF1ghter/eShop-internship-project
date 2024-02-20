using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces.Repos;
using Catalogs.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Infrastructure.Repos
{
    public sealed class ItemRepository(CatalogContext context) : Repository<Item>(context), IItemRepository
    {
        private readonly CatalogContext _context = context;

        public async Task<IEnumerable<Item>> GetAllItemsAsync(bool trackChanges) =>
            await GetAll(trackChanges)
                    .ToListAsync();

        public async Task<Item?> GetItemByIdAsync(int id,  bool trackChanges) => 
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
