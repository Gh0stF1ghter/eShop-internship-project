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
    public sealed class BrandRepository(CatalogContext context) : Repository<Brand>(context), IBrandRepository
    {
        private readonly CatalogContext _сontext = context;

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync(bool trackChanges) =>
            await GetAll(trackChanges)
                    .OrderBy(b => b.Name)
                    .ToListAsync();

        public async Task<Brand?> GetBrandByIdAsync(int id, bool trackChanges) => 
            await GetByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public void DeleteBrand(Brand brand) => Delete(brand);
    }
}
