using Bogus;
using Catalogs.Domain.RequestFeatures;

namespace Catalogs.Tests.UnitTests.FakeDataGenerator
{
    internal static class DataGenerator
    {
        private const int CountOfGeneratedUnits = 2;

        public static readonly PagedList<Item> Items = new([], 1, 1, 5);
        public static readonly List<Brand> Brands = [];
        public static readonly List<Vendor> Vendors = [];
        public static readonly List<ItemType> ItemTypes = [];

        public static void InitBogusData()
        {
            if (Items.Count > 0)
            {
                return;
            }

            GenerateBrandData();
            GenerateItemTypeData();
            GenerateVendorData();
        }

        private static void GenerateBrandData()
        {
            var generator = GetBrandGenerator();
            var generatedItems = generator.Generate(CountOfGeneratedUnits);

            Brands.AddRange(generatedItems);
        }

        private static void GenerateItemTypeData()
        {
            var generator = GetItemTypeGenerator();
            var generatedItems = generator.Generate(CountOfGeneratedUnits);

            ItemTypes.AddRange(generatedItems);
        }

        private static void GenerateVendorData()
        {
            var generator = GetVendorGenerator();
            var generatedItems = generator.Generate(CountOfGeneratedUnits);

            Vendors.AddRange(generatedItems);
        }

        private static Faker<Vendor> GetVendorGenerator() =>
            new Faker<Vendor>()
                .RuleFor(v => v.Id, f => f.UniqueIndex)
                .RuleFor(v => v.Name, f => f.Company.CompanyName());

        private static Faker<ItemType> GetItemTypeGenerator() =>
            new Faker<ItemType>()
                .RuleFor(it => it.Id, f => f.UniqueIndex)
                .RuleFor(it => it.Name, f => f.Commerce.Categories(1)[0]);

        private static Faker<Item> GetItemGeneratorForBrand(int brandId) =>
            new Faker<Item>()
                .RuleFor(i => i.Id, f => f.UniqueIndex)
                .RuleFor(i => i.BrandId, brandId)
                .RuleFor(i => i.Name, f => f.Commerce.Product());

        private static Faker<Brand> GetBrandGenerator() =>
            new Faker<Brand>()
                .RuleFor(b => b.Id, f => f.UniqueIndex)
                .RuleFor(b => b.Name, f => f.Company.CompanyName())
                .RuleFor(b => b.Items, (_, b) => GetBogusItemData(b.Id));

        private static List<Item> GetBogusItemData(int brandId)
        {
            var generator = GetItemGeneratorForBrand(brandId);
            var generatedItems = generator.Generate(CountOfGeneratedUnits);

            Items.AddRange(generatedItems);

            return generatedItems;
        }
    }
}