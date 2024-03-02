using Baskets.DataAccess.DBContext;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.Repositories.Implementations;
using Baskets.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Options;

namespace Baskets.DataAccess.UnitOfWork
{
    public class UnitOfWork(IMongoDbContext context, IOptions<BasketDatabaseSettings> options) : IUnitOfWork
    {
        private readonly Lazy<IRepository<UserBasket>> BasketRepository = new(new Repository<UserBasket>(context, options.Value.BasketsCollectionName));
        private readonly Lazy<IBasketItemRepository> BasketItemRepository = new(new BasketItemRepository(context, options));
        private readonly Lazy<IRepository<User>> UserRepository = new(new Repository<User>(context, options.Value.UsersCollectionName));
        private readonly Lazy<IRepository<Item>> ItemRepository = new(new Repository<Item>(context, options.Value.ItemsCollectionName));

        public IRepository<UserBasket> Basket => BasketRepository.Value;
        public IBasketItemRepository BasketItem => BasketItemRepository.Value;
        public IRepository<User> User => UserRepository.Value;
        public IRepository<Item> Item => ItemRepository.Value;
    }
}