using Catalogs.Domain.Interfaces.Repos;

namespace Catalogs.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBrandRepository Brands { get; }
        IItemRepository Items { get; }
        IVendorRepository Vendors { get; }
        IRepository<Type> Types { get; }

        void SaveChanges();
    }
}
