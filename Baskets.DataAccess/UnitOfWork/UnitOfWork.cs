using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.Repositories.Implementations;
using Baskets.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Baskets.DataAccess.UnitOfWork
{
    public class UnitOfWork(IMongoClient client, IOptions<BasketDatabaseSettings> options) : IUnitOfWork
    {
        private readonly Lazy<IRepository<UserBasket>> BasketRepository = new(new Repository<UserBasket>(client, options, options.Value.BasketsCollectionName));
        private readonly Lazy<IRepository<BasketItem>> BasketItemRepository = new(new Repository<BasketItem>(client, options, options.Value.BasketItemsCollectionName));
        private readonly Lazy<IRepository<User>> UserRepository = new(new Repository<User>(client, options, options.Value.UsersCollectionName));

        public IRepository<UserBasket> Basket => BasketRepository.Value;
        public IRepository<BasketItem> BasketItem => BasketItemRepository.Value;
        public IRepository<User> User => UserRepository.Value;
    }
}

