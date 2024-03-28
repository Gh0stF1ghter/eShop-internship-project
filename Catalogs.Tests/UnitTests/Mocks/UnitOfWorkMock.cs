using Catalogs.Domain.RequestFeatures;
using System.Threading;

namespace Catalogs.Tests.UnitTests.Mocks
{
    public class UnitOfWorkMock() : Mock<IUnitOfWork>()
    {
        private readonly CancellationToken cancellationToken = It.IsAny<CancellationToken>();

        public void IsBrandExists(bool expectedReturn) =>
            Setup(uof => uof.Brand.IsExistAsync(It.IsAny<Expression<Func<Brand, bool>>>(), cancellationToken))
                .ReturnsAsync(expectedReturn);

        public void IsItemExists(bool expectedReturn) =>
            Setup(uof => uof.Item.IsExistAsync(It.IsAny<Expression<Func<Item, bool>>>(), cancellationToken))
                .ReturnsAsync(expectedReturn);

        public void IsItemTypeExists(bool expectedReturn) =>
            Setup(uof => uof.ItemType.IsExistAsync(It.IsAny<Expression<Func<ItemType, bool>>>(), cancellationToken))
                .ReturnsAsync(expectedReturn);

        public void IsVendorExists(bool expectedReturn) =>
            Setup(uof => uof.Vendor.IsExistAsync(It.IsAny<Expression<Func<Vendor, bool>>>(), cancellationToken))
                .ReturnsAsync(expectedReturn);

        public void GetItemById(Item? item) =>
            Setup(uof => uof.Item.GetItemOfTypeByIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), cancellationToken))
                .ReturnsAsync(item);

        public void GetAllItems(PagedList<Item> items) =>
            Setup(uof => uof.Item.GetAllItemsOfTypeAsync(It.IsAny<int>(), It.IsAny<ItemParameters>(), It.IsAny<bool>(), cancellationToken)).
                ReturnsAsync(items);

        public void GetBrandById(Brand? brand) =>
            Setup(uof => uof.Brand.GetBrandByIdAsync(It.IsAny<int>(), It.IsAny<bool>(), cancellationToken))
                .ReturnsAsync(brand);

        public void GetAllBrands(List<Brand> brands) =>
            Setup(uof => uof.Brand.GetAllBrandsAsync(false, cancellationToken))
                .ReturnsAsync(brands);

        public void GetItemTypeById(ItemType? itemType) =>
            Setup(uof => uof.ItemType.GetItemTypeByIdAsync(It.IsAny<int>(), It.IsAny<bool>(), cancellationToken))
                .ReturnsAsync(itemType);

        public void GetAllItemTypes() =>
            Setup(uof => uof.ItemType.GetAllItemTypesAsync(false, cancellationToken))
                .ReturnsAsync(new List<ItemType>
                {
                    new()
                    {
                        Id = 1,
                        Name = "Foo",
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Boo",
                    },
                });

        public void GetVendorById(Vendor? vendor) =>
            Setup(uof => uof.Vendor.GetVendorByIdAsync(It.IsAny<int>(), It.IsAny<bool>(), cancellationToken))
                .ReturnsAsync(vendor);

        public void GetAllVendors() =>
            Setup(uof => uof.Vendor.GetAllVendorsAsync(false, cancellationToken))
                .ReturnsAsync(new List<Vendor>
                {
                    new()
                    {
                        Id = 1,
                        Name = "Foo",
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Boo",
                    },
                });
    }
}