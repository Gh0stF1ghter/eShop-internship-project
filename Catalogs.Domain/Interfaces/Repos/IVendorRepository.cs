using Catalogs.Domain.Entities.Models;
using System.Linq.Expressions;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IVendorRepository : IRepository<Vendor>
    {
        Task<IEnumerable<Vendor>> GetAllVendorsAsync(bool trackChanges);
        Task<Vendor?> GetVendorByIdAsync(int id, bool trackChanges);
    }
}
