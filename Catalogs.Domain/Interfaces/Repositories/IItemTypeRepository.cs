using Catalogs.Domain.Entities.Models;

namespace Catalogs.Domain.Interfaces.Repositories
{
    public interface IItemTypeRepository : IRepository<ItemType>
    {
        Task<IEnumerable<ItemType>> GetAllItemTypesAsync(bool trackChanges, CancellationToken token);
        Task<ItemType?> GetItemTypeByIdAsync(int id, bool trackChanges, CancellationToken token);
    }
}
