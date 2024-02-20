using Catalogs.Domain.Entities.Models;
using System.Linq.Expressions;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetAllItemsAsync(bool trackChanges);

        Task<Item?> GetItemByIdAsync(Guid id, bool trackChanges);

        Task<Vendor> GetItemByConditionAsync(Expression<Func<Vendor, bool>> condition, bool trackChanges);

        Task AddItemAsync(Guid brandId, Guid typeId, Guid Vendor, Item item);
    }
}
