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
using Baskets.UnitTests.Mock;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace Baskets.UnitTests.HandlersTests
{
    public class BasketItemHandlersTests
    {
        private readonly UnitOfWorkMock _unitOfWorkMock = new();

        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc => mc.AddProfiles(
            [
                new BasketItemProfile(),
                new ItemProfile(),
                new UserBasketProfile()
            ])));

        public BasketItemHandlersTests()
        {
            _unitOfWorkMock.GetBasketByCondition(FakeDb.Baskets[0]);

            _unitOfWorkMock.GetAllBasketItems(FakeDb.BasketItems);
        }

        [Fact]
        public async Task GetBasketItems_ValidParameters_ReturnsList()
        {
            //Arrange
            var getBasketItemsQuery = new GetBasketItemsQuery("65e0f6b92fa24267a5c3fa13");
            var handler = new GetBasketItemsHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(getBasketItemsQuery, cancellationToken: default);

            //Assert
            response.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetBasketItems_InvalidBasketId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var getBasketItemsQuery = new GetBasketItemsQuery("65e0f6b92fa24267a5c3fa13");
            var handler = new GetBasketItemsHandler(_unitOfWorkMock.Object, _mapper);

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
        public async Task GetBasketItem_ValidParameters_ReturnsBasketItem(string id)
        {
            //Arrange
            var mockedResult = FakeDb.BasketItems[0];

            _unitOfWorkMock.GetBasketItemWithItemByCondition(mockedResult);

            var getBasketItemQuery = new GetBasketItemQuery("65e0f6b92fa24267a5c3fa13", id);
            var handler = new GetBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(getBasketItemQuery, cancellationToken: default);

            //Assert
            response.Id.Should()
                .BeEquivalentTo(mockedResult.Id);
        }

        [Theory]
        [InlineData("65e0f6b92fa24267a5c3fa13", null, BasketItemMessages.NotFound)]
        [InlineData(null, "65e2122401130591c38d52e3", UserBasketMessages.NotFound)]
        public async Task GetBasketItem_InvalidId_ThrowsNotFoundException(string userId, string basketItemId, string responseMessage)
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(FakeDb.Baskets.Find(b => b.UserId.Equals(userId)));

            _unitOfWorkMock.GetBasketItemWithItemByCondition(FakeDb.BasketItems.Find(bi => bi.Id.Equals(basketItemId)));

            var getBasketItemQuery = new GetBasketItemQuery(userId, basketItemId);
            var handler = new GetBasketItemHandler(_unitOfWorkMock.Object, _mapper);

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
        public async Task CreateBasketItem_ValidParameters_ReturnsBasketItem(string itemId)
        {
            //Arrange
            var mockedItem = FakeDb.Items.Find(bi => bi.Id.Equals(itemId));

            _unitOfWorkMock.GetItemByCondition(mockedItem);

            _unitOfWorkMock.GetBasketItemWithItemByCondition(
                FakeDb.BasketItems.Find(bi => bi.ItemId.Equals(itemId)
                && bi.UserId.Equals("65e0f6b92fa24267a5c3fa13")));

            var createBasketItemComand = new CreateBasketItemComand("65e0f6b92fa24267a5c3fa13", new(itemId));
            var handler = new CreateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(createBasketItemComand, cancellationToken: default);

            //Assert
            response.SumPrice.Should()
                .BeOneOf(mockedItem.Price);
        }

        [Fact]
        public async Task CreateBasketItem_ExistingItemId_ThrowsAlreadyExistsException()
        {
            //Arrange
            _unitOfWorkMock.GetItemByCondition(FakeDb.Items.Find(bi => bi.Id.Equals("65e0f44d2fa24267a5c3fa07")));

            _unitOfWorkMock.GetBasketItemByCondition(
                FakeDb.BasketItems.Find(bi => bi.ItemId.Equals("65e0f44d2fa24267a5c3fa07")
                    && bi.UserId.Equals("65e0f6b92fa24267a5c3fa13")));

            var createBasketItemComand = new CreateBasketItemComand("65e0f6b92fa24267a5c3fa13", new("65e0f44d2fa24267a5c3fa07"));
            var handler = new CreateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(createBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<AlreadyExistsException>()
                .WithMessage(BasketItemMessages.Exists);
        }

        [Theory]
        [InlineData("65e0f44d2fa24267a5c3fa07", "65e0f6b92fa24267a5c3fa16", UserMessages.NotFound)]
        [InlineData("65e0f6b92fa24267a5c3fa13", "65e0f6b92fa24267a5c3fa14", ItemMessages.NotFound)]
        public async Task CreateBasketItem_InvalidUserOrItemIds_ThrowsNotFoundException(string itemId, string userId, string responseMessage)
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(FakeDb.Baskets.Find(bi => bi.UserId.Equals(userId)));

            _unitOfWorkMock.GetItemByCondition(FakeDb.Items.Find(bi => bi.Id.Equals(itemId)));

            var createBasketItemComand = new CreateBasketItemComand("65e0f6b92fa24267a5c3fa13", new("65e0f44d2fa24267a5c3fa07"));
            var handler = new CreateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(createBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(responseMessage);
        }

        [Fact]
        public async Task DeleteBasketItem_ValidParameters_ChangesBasketPrice()
        {
            //Arrange
            UserBasket basket = FakeDb.Baskets.First();

            _unitOfWorkMock.DeleteBasketItem(FakeDb.BasketItems.Find(bi => bi.Id.Equals("65e2122401130591c38d52e3") && bi.UserId.Equals("65e0f6b92fa24267a5c3fa13")));

            var deleteBasketItemComand = new DeleteBasketItemComand("65e0f6b92fa24267a5c3fa13", "65e2122401130591c38d52e3");
            var handler = new DeleteBasketItemHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(deleteBasketItemComand, cancellationToken: default);
            
            //Assert
            await response.Should().NotThrowAsync();
        }

        [Theory]
        [InlineData("65e0f44d2fa24267a5c3fa07", "65e0f6b92fa24267a5c3fa16", UserBasketMessages.NotFound)]
        [InlineData("65e0f6b92fa24267a5c3fa13", "65eebaf62141340b41b7052a", BasketItemMessages.NotFound)]
        public async void DeleteBasketItem_InvalidUserOrItemIds_ThrowsNotFoundException(string userId, string basketItemId, string expectedResponse)
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(FakeDb.Baskets.Find(bi => bi.UserId.Equals(userId)));

            _unitOfWorkMock.GetBasketItemWithItemByCondition(FakeDb.BasketItems.Find(bi => bi.Id.Equals(basketItemId)));

            var deleteBasketItemComand = new DeleteBasketItemComand("65e0f6b92fa24267a5c3fa13", "65e2122401130591c38d52e3");
            var handler = new DeleteBasketItemHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(deleteBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(expectedResponse);
        }

        [Fact]
        public async Task UpdateBasketItem_ValidParameters_ChangesItemCost()
        {
            //Arrange
            var basketItemMock = FakeDb.BasketItems.Find(bi => bi.Id.Equals("65e2122401130591c38d52e3"));
            var basketMock = FakeDb.Baskets[0];

            _unitOfWorkMock.GetBasketItemWithItemByCondition(basketItemMock);

            var updateBasketItem = new UpdateBasketItemComand("65e0f6b92fa24267a5c3fa13", "65e2122401130591c38d52e3", 3);
            var handler = new UpdateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(updateBasketItem, cancellationToken: default);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Theory]
        [InlineData("65e0f6b92fa24267a5c3fa13", "65e2122401130591c38d5213", BasketItemMessages.NotFound)]
        [InlineData("65e0f6b92fa24267a5c3fa19", "65e2122401130591c38d52e3", UserBasketMessages.NotFound)]
        public async Task UpdateBasketItem_InvalidUserOrItemIds_ThrowsNotFoundException(string userId, string basketItemId, string responseMessage)
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(FakeDb.Baskets.Where(bi => bi.UserId.Equals(userId)).FirstOrDefault());

            _unitOfWorkMock.GetBasketItemWithItemByCondition(FakeDb.BasketItems.Find(bi => bi.Id.Equals(basketItemId)));

            var updateBasketItem = new UpdateBasketItemComand(userId, basketItemId, 3);
            var handler = new UpdateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(updateBasketItem, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(responseMessage);
        }
    }
}