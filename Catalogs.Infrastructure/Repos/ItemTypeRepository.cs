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
    public sealed class ItemTypeRepository(CatalogContext context) : Repository<ItemType>(context), IItemTypeRepository
    {
        private readonly CatalogContext _context = context;

        public async Task<IEnumerable<ItemType>> GetAllItemTypesAsync(bool trackChanges) =>
            await GetAll(trackChanges).ToListAsync();

        public async Task<ItemType?> GetItemTypeByIdAsync(int id, bool trackChanges) =>
            await GetByCondition(t => t.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
    }
}
