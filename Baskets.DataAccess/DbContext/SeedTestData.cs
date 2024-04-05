using Baskets.DataAccess.Entities.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Baskets.DataAccess.DbContext
{
    public class SeedTestData(IMongoDatabase database, IOptions<BasketDatabaseSettings> options)
    {
        private readonly IMongoCollection<User> _users = database.GetCollection<User>(options.Value.UsersCollectionName);
        private readonly IMongoCollection<Item> _items = database.GetCollection<Item>(options.Value.ItemsCollectionName);
        private readonly IMongoCollection<UserBasket> _baskets = database.GetCollection<UserBasket>(options.Value.BasketsCollectionName);
        private readonly IMongoCollection<BasketItem> _basketItems = database.GetCollection<BasketItem>(options.Value.BasketItemsCollectionName);

        public void Seed()
        {
            var isUsersCollectionFilled = _users.CountDocuments(_ => true);

            if (isUsersCollectionFilled == 0)
            {
                AddUsers();
            }

            var isItemsCollectionFilled = _items.CountDocuments(_ => true);

            if (isItemsCollectionFilled == 0)
            {
                AddItems();
            }

            var isBasketsCollectionFilled = _baskets.CountDocuments(_ => true);

            if (isBasketsCollectionFilled == 0)
            {
                AddUserBaskets();
            }

            var isBasketItemsCollectionFilled = _basketItems.CountDocuments(_ => true);

            if (isBasketItemsCollectionFilled == 0)
            {
                AddBasketItems();
            }
        }

        private void AddUsers()
        {
            _users.InsertMany([
                new User
                {
                    Id = "65faeac0eb349fe87cd2f279",
                    UserId = 1,
                },
                new User
                {
                    UserId = 2
                },
                new User
                {
                    Id = "65faeac0eb349fe87cd2f27b",
                    UserId = 3
                }]);
        }

        private void AddItems()
        {
            _items.InsertMany([
                new Item
                {
                    Id = "65faedae6e5985f2047051af",
                    ItemId = 1,
                    Name = "phone",
                    Price = 500.00,
                    ImageUrl = "phone.jpeg"
                },
                new Item
                {
                    Id = "65faedae6e5985f2047051b0",
                    ItemId = 2,
                    Name = "tablet",
                    Price = 750.00,
                    ImageUrl = "tablet.jpeg"
                },
                new Item
                {
                    Id = "65faedae6e5985f2047051b1",
                    ItemId = 3,
                    Name = "book",
                    Price = 25.00,
                    ImageUrl = "book.jpeg"
                },
            ]);
        }

        private void AddUserBaskets()
        {
            _baskets.InsertMany([
                new UserBasket {
                    UserId = "65faeac0eb349fe87cd2f27a",
                    TotalPrice = 0
                },
                new UserBasket {
                    UserId = "65faeac0eb349fe87cd2f279",
                    TotalPrice = 1250
                }
            ]);
        }

        private void AddBasketItems()
        {
            _basketItems.InsertMany([
                new BasketItem {
                    BasketItemId = "65fb182586219380db88b9fc",
                    Quantity = 1,
                    SumPrice = 500,
                    UserId = "65faeac0eb349fe87cd2f279",
                    ItemId = "65faedae6e5985f2047051af"
                },
                new BasketItem {
                    BasketItemId = "65fb185d86219380db88b9fd",
                    Quantity = 1,
                    SumPrice = 750,
                    UserId = "65faeac0eb349fe87cd2f279",
                    ItemId = "65faedae6e5985f2047051b0"
                },
            ]);
        }
    }
}