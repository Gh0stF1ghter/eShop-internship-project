using Catalogs.Domain.Entities.Models;
using System.Linq.Expressions;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IVendorRepository : IRepository<Vendor>
    {
        Task<IQueryable<Vendor>> GetAllVendorsAsync();

        Task<Vendor> GetVendorByIdAsync(int id);

        Task<Vendor> GetVendorByConditionAsync(Expression<Func<Vendor, bool>> condition, bool trackChanges);
    }
}
