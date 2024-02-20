using Catalogs.Domain.Entities.Models;
using System.Linq.Expressions;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetAllItemsAsync(bool trackChanges);
        Task<Item?> GetItemByIdAsync(int id, bool trackChanges);

        void AddItem(int brandId, int typeId, int Vendor, Item item);
    }
}
