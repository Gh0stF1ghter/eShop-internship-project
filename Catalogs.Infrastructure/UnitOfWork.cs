using Catalogs.Domain.Interfaces;
using Catalogs.Domain.Interfaces.Repos;
using Catalogs.Infrastructure.Context;
using Catalogs.Infrastructure.Repos;

namespace Catalogs.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatalogContext _context;

        private readonly Lazy<IBrandRepository> _brandRepository;
        private readonly Lazy<IItemRepository> _itemRepository;
        private readonly Lazy<IItemTypeRepository> _itemTypeRepository;
        private readonly Lazy<IVendorRepository> _vendorRepository;

        public UnitOfWork(CatalogContext context)
        {
            _context = context;
            _brandRepository = new Lazy<IBrandRepository>(new BrandRepository(_context));
            _itemRepository = new Lazy<IItemRepository>(new ItemRepository(_context));
            _itemTypeRepository = new Lazy<IItemTypeRepository>(new ItemTypeRepository(_context));
            _vendorRepository = new Lazy<IVendorRepository>(new VendorRepository(_context));
        }

        public IBrandRepository Brand => _brandRepository.Value;
        public IItemRepository Item => _itemRepository.Value;
        public IItemTypeRepository ItemType => _itemTypeRepository.Value;
        public IVendorRepository Vendor => _vendorRepository.Value;

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _context.Dispose();
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
