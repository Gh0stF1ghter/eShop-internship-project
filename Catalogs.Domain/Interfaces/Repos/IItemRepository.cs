using Catalogs.Domain.Entities.Models;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetAllItemsOfTypeAsync(int typeId, bool trackChanges, CancellationToken token);
        Task<Item?> GetItemOfTypeByIdAsync(int typeId, int id, bool trackChanges, CancellationToken token);

        void AddItem(int typeId, Item item);
    }
}
