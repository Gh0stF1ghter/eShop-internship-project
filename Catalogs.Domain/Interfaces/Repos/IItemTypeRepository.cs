using Catalogs.Domain.Entities.Models;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IItemTypeRepository : IRepository<ItemType>
    {
        Task<IEnumerable<ItemType>> GetAllItemTypesAsync(bool trackChanges, CancellationToken token);
        Task<ItemType?> GetItemTypeByIdAsync(int id, bool trackChanges, CancellationToken token);
    }
}
