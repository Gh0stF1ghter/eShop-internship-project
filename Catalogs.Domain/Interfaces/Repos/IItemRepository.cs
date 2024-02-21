using Catalogs.Domain.Entities.Models;
using System.Linq.Expressions;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetAllItemsAsync(bool trackChanges);
        Task<IEnumerable<Item>> GetAllItemsOfBrandAsync(int brandId, bool trackChanges);
        Task<IEnumerable<Item>> GetAllItemsOfTypeAsync(int typeId, bool trackChanges);
        Task<IEnumerable<Item>> GetAllItemsOfVendorAsync(int vendorId, bool trackChanges);

        Task<Item?> GetItemByIdAsync(int id, bool trackChanges);

        void AddItem(int brandId, int typeId, int Vendor, Item item);
    }
}
