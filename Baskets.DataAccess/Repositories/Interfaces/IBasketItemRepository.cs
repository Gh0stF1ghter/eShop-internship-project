using Baskets.DataAccess.Entities.Models;
using System.Linq.Expressions;

namespace Baskets.DataAccess.Repositories.Interfaces
{
    public interface IBasketItemRepository : IRepository<BasketItem>
    {
        Task<IEnumerable<BasketItem>> GetAllBasketItemsAsync(string basketId, CancellationToken cancellationToken);
        Task<BasketItem?> GetBasketItemByConditionAsync(Expression<Func<BasketItem, bool>> condition, CancellationToken cancellationToken);
    }
}
