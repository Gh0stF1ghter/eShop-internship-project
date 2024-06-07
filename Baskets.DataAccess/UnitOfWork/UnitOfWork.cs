using Baskets.DataAccess.DBContext;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.Repositories.Implementations;
using Baskets.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Options;

namespace Baskets.DataAccess.UnitOfWork
{
    public class UnitOfWork(IMongoDbContext context, IOptions<BasketDatabaseSettings> options) : IUnitOfWork
    {
        private readonly Lazy<IRepository<UserBasket>> BasketRepository = new(new Repository<UserBasket>(context.GetCollection<UserBasket>(options.Value.BasketsCollectionName)));
        private readonly Lazy<IBasketItemRepository> BasketItemRepository = new(new BasketItemRepository(context.GetCollection<BasketItem>(options.Value.BasketItemsCollectionName), context.GetCollection<Item>(options.Value.ItemsCollectionName)));
        private readonly Lazy<IRepository<Item>> ItemRepository = new(new Repository<Item>(context.GetCollection<Item>(options.Value.ItemsCollectionName)));

        public IRepository<UserBasket> Basket => BasketRepository.Value;
        public IBasketItemRepository BasketItem => BasketItemRepository.Value;
        public IRepository<Item> Item => ItemRepository.Value;
    }
}