using Catalogs.Domain.Entities.Models;

namespace Catalogs.Domain.Interfaces.Repositories
{
    public interface IVendorRepository : IRepository<Vendor>
    {
        Task<IEnumerable<Vendor>> GetAllVendorsAsync(bool trackChanges, CancellationToken token);
        Task<Vendor?> GetVendorByIdAsync(int id, bool trackChanges, CancellationToken token);
        Task<Vendor> GetVendorToUpdateAsync(int id, CancellationToken token);

        Task AddVendorAsync(Vendor vendor, CancellationToken token);
        Task DeleteVendorAsync(Vendor vendor, CancellationToken token);
    }
}
