using Baskets.DataAccess.Entities.Models;

namespace Baskets.Tests.UnitTests.BasketFakeDb
{
    internal static class DataGenerator
    {
        public static readonly List<BasketItem> BasketItems = [];
        public static readonly List<UserBasket> UserBaskets = [];
        public static readonly List<Item> Items = [];
        public static readonly List<User> Users = [];

        public static void InitBogusData()
        {
            if (BasketItems.Count > 0)
            {
                return;
            }

            GenerateBasketItems();
            GenerateUserBaskets();
        }

        private static void GenerateUserBaskets()
        {
            var generator = GetUserBasketGenerator();
            var user = generator.Generate(2);

            UserBaskets.AddRange(user);
        }

        private static void GenerateBasketItems()
        {
            var generator = GetBasketItemGenerator();
            var basket = generator.Generate(2);

            BasketItems.AddRange(basket);
        }

        private static Faker<Item> GetItemGenerator(string itemId) =>
            new Faker<Item>()
                .RuleFor(i => i.Id, itemId)
                .RuleFor(i => i.Name, f => f.Commerce.Product());

        private static Faker<User> GetUserGenerator(string userId) =>
            new Faker<User>()
                .RuleFor(u => u.Id, userId);

        private static Faker<BasketItem> GetBasketItemGenerator() =>
            new Faker<BasketItem>()
                .RuleFor(bi => bi.UserId, f => f.Random.String(24, minChar: '0', maxChar: 'F'))
                .RuleFor(bi => bi.BasketItemId, f => f.Random.String(24, minChar: '0', maxChar: 'F'))
                .RuleFor(bi => bi.ItemId, f => f.Random.String(24, minChar: '0', maxChar: 'F'))
                .RuleFor(bi => bi.Item, (_, bi) => GetBogusItemData(bi.ItemId));

        private static Faker<UserBasket> GetUserBasketGenerator() =>
            new Faker<UserBasket>()
                .RuleFor(ub => ub.UserId, f => f.Random.String(24, minChar: '0', maxChar: 'F'))
                .RuleFor(ub => ub.User, (_, bi) => GetBogusUserData(bi.UserId));

        private static User GetBogusUserData(string userId)
        {
            var generator = GetUserGenerator(userId);
            var user = generator.Generate();

            Users.Add(user);

            return user;
        }

        private static Item GetBogusItemData(string itemId)
        {
            var generator = GetItemGenerator(itemId);
            var user = generator.Generate();

            Items.Add(user);

            return user;
        }
    }
}