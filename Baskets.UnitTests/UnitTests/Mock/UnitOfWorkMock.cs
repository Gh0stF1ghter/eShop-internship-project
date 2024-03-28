using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.UnitOfWork;
using System.Linq.Expressions;

namespace Baskets.Tests.UnitTests.Mock
{
    internal class UnitOfWorkMock : Mock<IUnitOfWork>
    {
        public void GetBasketByCondition(UserBasket? basket) =>
            Setup(unitOfWork => unitOfWork.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(basket);

        public void DeleteBasket(UserBasket? basket) =>
            Setup(unitOfWork => unitOfWork.Basket.DeleteAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(basket);

        public void GetAllBasketItems(IEnumerable<BasketItem> items) =>
            Setup(x => x.BasketItem.GetAllBasketItemsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(items);

        public void GetBasketItemWithItemByCondition(BasketItem? basketItem) =>
            Setup(uof => uof.BasketItem.GetBasketItemByConditionAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(basketItem);

        public void GetItemByCondition(Item? item) =>
            Setup(uof => uof.Item.GetByConditionAsync(It.IsAny<Expression<Func<Item, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(item);

        public void DeleteBasketItem(BasketItem? basketItem) =>
            Setup(uof => uof.BasketItem.DeleteAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(basketItem);

        public void GetBasketItemByCondition(BasketItem? basketItem) =>
            Setup(uof => uof.BasketItem.GetByConditionAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(basketItem);

        public void GetUserByCondition(User? user) =>
            Setup(uof => uof.User.GetByConditionAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
    }
}