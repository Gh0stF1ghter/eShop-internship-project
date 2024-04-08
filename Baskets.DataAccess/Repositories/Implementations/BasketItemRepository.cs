using Baskets.DataAccess.DBContext;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Baskets.DataAccess.Repositories.Implementations
{
    public class BasketItemRepository(IMongoCollection<BasketItem> basketCollection, IMongoCollection<Item> _itemCollection) : Repository<BasketItem>(basketCollection), IBasketItemRepository
    {
        public async Task<IEnumerable<BasketItem>> GetAllBasketItemsAsync(string basketId, CancellationToken cancellationToken)
        {
            var basketItems = await GetAllByConditionAsync(bi => bi.UserId.Equals(basketId), cancellationToken);

            return basketItems;
        }

        public async Task<BasketItem?> GetBasketItemByConditionAsync(Expression<Func<BasketItem, bool>> condition, CancellationToken cancellationToken)
        {
            var basketItem = await GetByConditionAsync(condition, cancellationToken);

            return basketItem;
        }
    }
}
