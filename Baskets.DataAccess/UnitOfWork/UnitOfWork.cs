using Baskets.DataAccess.DBContext;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.Repositories.Implementations;
using Baskets.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Baskets.DataAccess.UnitOfWork
{
    public class UnitOfWork(IMongoDbContext context, IOptions<BasketDatabaseSettings> options) : IUnitOfWork
    {
        private readonly Lazy<IRepository<UserBasket>> BasketRepository = new(new Repository<UserBasket>(context, options.Value.BasketsCollectionName));
        private readonly Lazy<IRepository<BasketItem>> BasketItemRepository = new(new Repository<BasketItem>(context, options.Value.BasketItemsCollectionName));
        private readonly Lazy<IRepository<User>> UserRepository = new(new Repository<User>(context, options.Value.UsersCollectionName));
        private readonly Lazy<IRepository<Item>> ItemRepository = new(new Repository<Item>(context, options.Value.ItemsCollectionName));

        public IRepository<UserBasket> Basket => BasketRepository.Value;
        public IRepository<BasketItem> BasketItem => BasketItemRepository.Value;
        public IRepository<User> User => UserRepository.Value;
        public IRepository<Item> Item => ItemRepository.Value;
    }
}