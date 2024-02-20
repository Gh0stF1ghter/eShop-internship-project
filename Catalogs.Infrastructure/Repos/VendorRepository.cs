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
    public sealed class VendorRepository(CatalogContext context) : Repository<Vendor>(context), IVendorRepository
    {
        private readonly CatalogContext _context = context;

        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync(bool trackChanges) =>
            await GetAll(trackChanges).ToListAsync();

        public async Task<Vendor?> GetVendorByIdAsync(int id, bool trackChanges) => 
            await GetByCondition(v => v.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
    }
}
