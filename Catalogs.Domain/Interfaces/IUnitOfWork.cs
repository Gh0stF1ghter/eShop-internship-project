using Catalogs.Domain.Interfaces.Repositories;

namespace Catalogs.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBrandRepository Brand { get; }
        IItemRepository Item { get; }
        IItemTypeRepository ItemType { get; }
        IVendorRepository Vendor { get; }

        Task SaveChangesAsync(CancellationToken token);
    }
}
