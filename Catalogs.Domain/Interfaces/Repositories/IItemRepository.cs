using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.RequestFeatures;

namespace Catalogs.Domain.Interfaces.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<PagedList<Item>> GetAllItemsOfTypeAsync(int typeId, ItemParameters itemParameters, bool trackChanges, CancellationToken token);
        Task<Item?> GetItemOfTypeByIdAsync(int typeId, int id, bool trackChanges, CancellationToken token);

        Task<Item?> GetItemByIdAsync(int id, CancellationToken token);

        void AddItem(int typeId, Item item);
    }
}
