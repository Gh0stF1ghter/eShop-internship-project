using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.Repositories.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Baskets.DataAccess.Repositories.Implementations
{
    public class BasketItemRepository(IMongoCollection<BasketItem> basketCollection, IMongoCollection<Item> _itemCollection) : Repository<BasketItem>(basketCollection), IBasketItemRepository
    {
        public async Task<IEnumerable<BasketItem>> GetAllBasketItemsAsync(string basketId, CancellationToken cancellationToken)
        {
            var basketItems = await GetAllByConditionAsync(bi => bi.UserId.Equals(basketId), cancellationToken);

            foreach (var basketItem in basketItems)
            {
                var item = await _itemCollection
                    .Find(i => i.Id.Equals(basketItem.ItemId))
                    .SingleOrDefaultAsync(cancellationToken);

                basketItem.Item = item;
            }

            return basketItems;
        }

        public async Task<BasketItem?> GetBasketItemByConditionAsync(Expression<Func<BasketItem, bool>> condition, CancellationToken cancellationToken)
        {
            var basketItem = await GetByConditionAsync(condition, cancellationToken);

            if (basketItem != null)
            {
                var item = await _itemCollection
                    .Find(i => i.Id.Equals(basketItem.ItemId))
                    .SingleOrDefaultAsync(cancellationToken);

                basketItem.Item = item;
            }

            return basketItem;
        }
    }
}