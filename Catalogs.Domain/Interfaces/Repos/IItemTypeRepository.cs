using Catalogs.Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Domain.Interfaces.Repos
{
    public interface IItemTypeRepository : IRepository<ItemType>
    {
        Task<IEnumerable<ItemType>> GetAllItemTypesAsync(bool trackChanges);
        Task<ItemType?> GetItemTypeByIdAsync(int id, bool trackChanges);
    }
}
