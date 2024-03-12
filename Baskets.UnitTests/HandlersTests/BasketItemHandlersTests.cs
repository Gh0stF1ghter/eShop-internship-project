using AutoMapper;
using Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.CreateBasketItemComand;
using Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.DeleteBasketItemComand;
using Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.UpdateBasketItemComand;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItemQuery;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItemsQuery;
using Baskets.BusinessLogic.Exceptions;
using Baskets.BusinessLogic.MappingProfiles;
using Baskets.DataAccess.Entities.Constants.Messages;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.UnitOfWork;
using Baskets.UnitTests.BasketFakeDb;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace Baskets.UnitTests.HandlersTests
{
    public class BasketItemHandlersTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc => mc.AddProfiles(
            [
                new BasketItemProfile(),
                new ItemProfile(),
                new UserBasketProfile()
            ])));

        public BasketItemHandlersTests()
        {
            _unitOfWork.Setup(x => x.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.Baskets.First);
            _unitOfWork.Setup(x => x.BasketItem.GetAllBasketItemsAsync("65e0f6b92fa24267a5c3fa13", It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.BasketItems.Where(bi => bi.UserId.Equals("65e0f6b92fa24267a5c3fa13")));
        }

        [Fact]
        public async Task GetBasketItemsReturnsList()
        {
            //Arrange
            var getBasketItemsQuery = new GetBasketItemsQuery("65e0f6b92fa24267a5c3fa13");
            var handler = new GetBasketItemsHandler(_unitOfWork.Object, _mapper);

            //Act
            var response = await handler.Handle(getBasketItemsQuery, cancellationToken: default);

            //Assert
            response.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetBasketItemsThrowsNotFoundException()
        {
            //Arrange
            _unitOfWork.Setup(uof => uof.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserBasket)null);

            var getBasketItemsQuery = new GetBasketItemsQuery("65e0f6b92fa24267a5c3fa13");
            var handler = new GetBasketItemsHandler(_unitOfWork.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(getBasketItemsQuery, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }

        [Theory]
        [InlineData("65eebaf62141340b41b7052a")]
        [InlineData("65e2122401130591c38d52e3")]
        public async Task GetBasketItemReturnsBasketItem(string id)
        {
            //Arrange
            var mockedResult = FakeDb.BasketItems.Where(bi => bi.Id.Equals(id)).First();

            _unitOfWork.Setup(uof => uof.BasketItem.GetBasketItemByConditionAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockedResult);

            var getBasketItemQuery = new GetBasketItemQuery("65e0f6b92fa24267a5c3fa13", id);
            var handler = new GetBasketItemHandler(_unitOfWork.Object, _mapper);

            //Act
            var response = await handler.Handle(getBasketItemQuery, cancellationToken: default);

            //Assert
            response.Id.Should()
                .BeEquivalentTo(mockedResult.Id);
        }

        [Theory]
        [InlineData("65e0f6b92fa24267a5c3fa13", null, BasketItemMessages.NotFound)]
        [InlineData(null, "65e2122401130591c38d52e3", UserBasketMessages.NotFound)]
        public async Task GetBasketItemThrowsNotFoundException(string userId, string basketItemId, string responseMessage)
        {
            //Arrange
            _unitOfWork.Setup(x => x.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.Baskets.Where(b => b.UserId.Equals(userId)).FirstOrDefault());
            _unitOfWork.Setup(uof => uof.BasketItem.GetBasketItemByConditionAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.BasketItems.Where(bi => bi.Id.Equals(basketItemId)).FirstOrDefault());

            var getBasketItemQuery = new GetBasketItemQuery(userId, basketItemId);
            var handler = new GetBasketItemHandler(_unitOfWork.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(getBasketItemQuery, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(responseMessage);
        }

        [Theory]
        [InlineData("65e0f431c871865e8372ae03")]
        [InlineData("65e0f47e2fa24267a5c3fa09")]
        public async Task CreateBasketItemReturnsBasketItem(string itemId)
        {
            var mockedItem = FakeDb.Items.Where(bi => bi.Id.Equals(itemId)).First();

            _unitOfWork.Setup(uof => uof.Item.GetByConditionAsync(It.IsAny<Expression<Func<Item, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockedItem);
            _unitOfWork.Setup(uof => uof.BasketItem.GetByConditionAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.BasketItems.Where(bi => bi.ItemId.Equals(itemId) && bi.UserId.Equals("65e0f6b92fa24267a5c3fa13")).FirstOrDefault());

            var createBasketItemComand = new CreateBasketItemComand("65e0f6b92fa24267a5c3fa13", new(itemId));
            var handler = new CreateBasketItemHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(createBasketItemComand, cancellationToken: default);

            response.SumPrice.Should()
                .BeOneOf(mockedItem.Price);
        }

        [Theory]
        [InlineData("65e0f44d2fa24267a5c3fa07")]
        [InlineData("65e0f45f2fa24267a5c3fa08")]
        public async Task CreateBasketItemThrowsAlreadyExistsException(string itemId)
        {
            _unitOfWork.Setup(uof => uof.Item.GetByConditionAsync(It.IsAny<Expression<Func<Item, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.Items.Where(bi => bi.Id.Equals(itemId)).First());
            _unitOfWork.Setup(uof => uof.BasketItem.GetByConditionAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.BasketItems.Where(bi => bi.ItemId.Equals(itemId) && bi.UserId.Equals("65e0f6b92fa24267a5c3fa13")).First());

            var createBasketItemComand = new CreateBasketItemComand("65e0f6b92fa24267a5c3fa13", new(itemId));
            var handler = new CreateBasketItemHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(createBasketItemComand, cancellationToken: default);

            await response.Should()
                .ThrowAsync<AlreadyExistsException>()
                .WithMessage(BasketItemMessages.Exists);
        }

        [Theory]
        [InlineData("65e0f44d2fa24267a5c3fa07", "65e0f6b92fa24267a5c3fa16", UserMessages.NotFound)]
        [InlineData("65e0f6b92fa24267a5c3fa13", "65e0f6b92fa24267a5c3fa14", ItemMessages.NotFound)]
        public async Task CreateBasketItemThrowsNotFoundException(string itemId, string userId, string responseMessage)
        {
            _unitOfWork.Setup(uof => uof.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.Baskets.Where(bi => bi.UserId.Equals(userId)).FirstOrDefault());
            _unitOfWork.Setup(uof => uof.Item.GetByConditionAsync(It.IsAny<Expression<Func<Item, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.Items.Where(bi => bi.Id.Equals(itemId)).FirstOrDefault());

            var createBasketItemComand = new CreateBasketItemComand("65e0f6b92fa24267a5c3fa13", new("65e0f44d2fa24267a5c3fa07"));
            var handler = new CreateBasketItemHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(createBasketItemComand, cancellationToken: default);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(responseMessage);
        }

        [Fact]
        public async Task DeleteBasketItemChangesBasketPrice()
        {
            UserBasket basket = FakeDb.Baskets.First();

            _unitOfWork.Setup(uof => uof.BasketItem.DeleteAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.BasketItems.Where(bi => bi.Id.Equals("65e2122401130591c38d52e3") && bi.UserId.Equals("65e0f6b92fa24267a5c3fa13")).First());
            _unitOfWork.Setup(uof => uof.Basket.Update(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<UserBasket>()));

            var deleteBasketItemComand = new DeleteBasketItemComand("65e0f6b92fa24267a5c3fa13", "65e2122401130591c38d52e3");
            var handler = new DeleteBasketItemHandler(_unitOfWork.Object);

            await handler.Handle(deleteBasketItemComand, cancellationToken: default);

            basket.TotalPrice.Should().BeOneOf(205);
        }

        [Theory]
        [InlineData("65e0f44d2fa24267a5c3fa07", "65e0f6b92fa24267a5c3fa16", UserBasketMessages.NotFound)]
        [InlineData("65e0f6b92fa24267a5c3fa13", "65eebaf62141340b41b7052a", BasketItemMessages.NotFound)]
        public async void DeleteBasketItemThrowsNotFoundException(string userId, string basketItemId, string expectedResponse)
        {
            _unitOfWork.Setup(uof => uof.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.Baskets.Where(bi => bi.UserId.Equals(userId)).FirstOrDefault());
            _unitOfWork.Setup(uof => uof.BasketItem.GetBasketItemByConditionAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.BasketItems.Where(bi => bi.Id.Equals(basketItemId)).FirstOrDefault());

            var deleteBasketItemComand = new DeleteBasketItemComand("65e0f6b92fa24267a5c3fa13", "65e2122401130591c38d52e3");
            var handler = new DeleteBasketItemHandler(_unitOfWork.Object);

            var response = async () => await handler.Handle(deleteBasketItemComand, cancellationToken: default);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(expectedResponse);
        }

        [Fact]
        public async Task UpdateBasketItemChangesItemCost()
        {
            var basketItemMock = FakeDb.BasketItems.Where(bi => bi.Id.Equals("65e2122401130591c38d52e3")).First();
            var basketMock = FakeDb.Baskets.First();

            _unitOfWork.Setup(uof => uof.BasketItem.GetBasketItemByConditionAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(basketItemMock);

            _unitOfWork.Setup(uof => uof.Basket.Update(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<UserBasket>()));
            _unitOfWork.Setup(uof => uof.BasketItem.Update(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<BasketItem>()));

            var updateBasketItem = new UpdateBasketItemComand("65e0f6b92fa24267a5c3fa13", "65e2122401130591c38d52e3", 3);
            var handler = new UpdateBasketItemHandler(_unitOfWork.Object, _mapper);

            await handler.Handle(updateBasketItem, cancellationToken: default);

            basketItemMock.SumPrice.Should().BeOneOf(1500);
        }

        [Theory]
        [InlineData("65e0f6b92fa24267a5c3fa13", "65e2122401130591c38d5213", BasketItemMessages.NotFound)]
        [InlineData("65e0f6b92fa24267a5c3fa19", "65e2122401130591c38d52e3", UserBasketMessages.NotFound)]
        public async Task UpdateBasketItemThrowsNotFoundException(string userId, string basketItemId, string responseMessage)
        {
            _unitOfWork.Setup(uof => uof.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.Baskets.Where(bi => bi.UserId.Equals(userId)).FirstOrDefault());
            _unitOfWork.Setup(uof => uof.BasketItem.GetBasketItemByConditionAsync(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.BasketItems.Where(bi => bi.Id.Equals(basketItemId)).FirstOrDefault);

            _unitOfWork.Setup(uof => uof.Basket.Update(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<UserBasket>()));
            _unitOfWork.Setup(uof => uof.BasketItem.Update(It.IsAny<Expression<Func<BasketItem, bool>>>(), It.IsAny<BasketItem>()));

            var updateBasketItem = new UpdateBasketItemComand(userId, basketItemId, 3);
            var handler = new UpdateBasketItemHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(updateBasketItem, cancellationToken: default);
        
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(responseMessage);
        }
    }
}