using Baskets.DataAccess.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.DataAccess.Repositories.Interfaces
{
    public interface IBasketItemRepository : IRepository<BasketItem>
    {
        Task<IEnumerable<BasketItem>> GetAllBasketItemsAsync(string basketId, CancellationToken cancellationToken);
        Task<BasketItem?> GetBasketItemByConditionAsync(Expression<Func<BasketItem, bool>> condition, CancellationToken cancellationToken);
    }
}
