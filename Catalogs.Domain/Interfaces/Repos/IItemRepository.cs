using Catalogs.Domain.Entities.Models;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetAllItemsOfTypeAsync(int typeId, bool trackChanges);
        Task<Item?> GetItemOfTypeByIdAsync(int typeId, int id, bool trackChanges);

        void AddItem(int brandId, Item item);
    }
}
