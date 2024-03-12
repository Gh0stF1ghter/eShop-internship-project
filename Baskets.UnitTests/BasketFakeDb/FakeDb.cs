using Baskets.DataAccess.Entities.Models;

namespace Baskets.UnitTests.BasketFakeDb
{
    public static class FakeDb
    {
        public static List<UserBasket> Baskets =
            [
                new()
                {
                    TotalPrice = 705,
                    UserId = "65e0f6b92fa24267a5c3fa13"
                }
            ];

        public static List<BasketItem> BasketItems =
            [
                new()
                {
                    Id = "65e2122401130591c38d52e3",
                    UserId = "65e0f6b92fa24267a5c3fa13",
                    ItemId = "65e0f45f2fa24267a5c3fa08",
                    SumPrice = 500,
                    Quantity = 1,
                    Item = new()
                    {
                        Id = "65e0f45f2fa24267a5c3fa08",
                        Name = "tablet",
                        Price = 500
                    }
                },
                new()
                {
                    Id = "65eebaf62141340b41b7052a",
                    UserId = "65e0f6b92fa24267a5c3fa13",
                    ItemId = "65e0f44d2fa24267a5c3fa07",
                    SumPrice = 205,
                    Quantity = 1,
                    Item = new()
                    {
                        Id = "65e0f44d2fa24267a5c3fa07",
                        Name = "phone",
                        Price = 205
                    }
                }
            ];

        public static List<Item> Items =
            [
                new()
                {
                    Id = "65e0f431c871865e8372ae03",
                    Name = "item",
                    Price = 500
                },
                new()
                {
                    Id = "65e0f44d2fa24267a5c3fa07",
                    Name = "phone",
                    Price = 205
                },
                new()
                {
                    Id = "65e0f45f2fa24267a5c3fa08",
                    Name = "tablet",
                    Price = 500
                },
                new()
                {
                    Id = "65e0f47e2fa24267a5c3fa09",
                    Name = "toothbrush",
                    Price = 120
                },
            ];
    }
}
