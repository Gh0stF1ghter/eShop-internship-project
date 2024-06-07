using Baskets.DataAccess.Entities.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Baskets.DataAccess.DbContext
{
    public class SeedTestData(IMongoDatabase database, IOptions<BasketDatabaseSettings> options)
    {
        private readonly IMongoCollection<Item> _items = database.GetCollection<Item>(options.Value.ItemsCollectionName);

        public void Seed()
        {
            var isItemsCollectionFilled = _items.CountDocuments(_ => true);

            if (isItemsCollectionFilled == 0)
            {
                AddItems();
            }
        }

        private void AddItems()
        {
            _items.InsertMany([
                new Item
                {
                    ItemId = 1,
                    Name = "phone",
                    Price = 500.00,
                    ImageUrl = "phone.jpeg"
                },
                new Item
                {
                    ItemId = 2,
                    Name = "tablet",
                    Price = 750.00,
                    ImageUrl = "tablet.jpeg"
                },
                new Item
                {
                    ItemId = 3,
                    Name = "book",
                    Price = 25.00,
                    ImageUrl = "book.jpeg"
                },
            ]);
        }
    }
}