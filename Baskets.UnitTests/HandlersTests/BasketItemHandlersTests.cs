using Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.CreateBasketItemComand;
using Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.DeleteBasketItemComand;
using Baskets.BusinessLogic.CQRS.Comands.BasketItemComands.UpdateBasketItemComand;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItemQuery;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItemsQuery;

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
            DataGenerator.InitBogusData();

            _unitOfWorkMock.GetBasketByCondition(DataGenerator.UserBaskets[0]);
            _unitOfWorkMock.GetAllBasketItems(DataGenerator.BasketItems);
        }

        [Fact]
        public async Task GetBasketItems_ValidParameters_ReturnsList()
        {
            //Arrange
            var getBasketItemsQuery = new GetBasketItemsQuery(It.IsAny<string>());
            var handler = new GetBasketItemsHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(getBasketItemsQuery, cancellationToken: default);

            //Assert
            response.Should().HaveCount(DataGenerator.BasketItems.Count);
        }

        [Fact]
        public async Task GetBasketItems_InvalidBasketId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var getBasketItemsQuery = new GetBasketItemsQuery(It.IsAny<string>());
            var handler = new GetBasketItemsHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(getBasketItemsQuery, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }

        [Fact]
        public async Task GetBasketItem_ValidParameters_ReturnsBasketItem()
        {
            //Arrange
            var mockedResult = DataGenerator.BasketItems[0];

            _unitOfWorkMock.GetBasketItemWithItemByCondition(mockedResult);

            var getBasketItemQuery = new GetBasketItemQuery(It.IsAny<string>(), It.IsAny<string>());
            var handler = new GetBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(getBasketItemQuery, cancellationToken: default);

            //Assert
            response.Id.Should()
                .BeEquivalentTo(mockedResult.Id);
        }

        [Fact]
        public async Task GetBasketItem_InvalidBasketId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var getBasketItemQuery = new GetBasketItemQuery(It.IsAny<string>(), It.IsAny<string>());
            var handler = new GetBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(getBasketItemQuery, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }

        [Fact]
        public async Task GetBasketItem_InvalidBasketItemId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketItemWithItemByCondition(null);

            var getBasketItemQuery = new GetBasketItemQuery(It.IsAny<string>(), It.IsAny<string>());
            var handler = new GetBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(getBasketItemQuery, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BasketItemMessages.NotFound);
        }


        [Fact]
        public async Task CreateBasketItem_ValidParameters_ReturnsBasketItem()
        {
            //Arrange
            var mockedItem = DataGenerator.Items[0];

            _unitOfWorkMock.GetItemByCondition(mockedItem);
            _unitOfWorkMock.GetBasketItemWithItemByCondition(null);

            var createBasketItemComand = new CreateBasketItemComand(It.IsAny<string>(), new(It.IsAny<string>()));
            var handler = new CreateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(createBasketItemComand, cancellationToken: default);

            //Assert
            response.SumPrice.Should()
                .Be(mockedItem.Price);
        }

        [Fact]
        public async Task CreateBasketItem_ExistingItemId_ThrowsAlreadyExistsException()
        {
            //Arrange
            _unitOfWorkMock.GetItemByCondition(DataGenerator.Items[0]);
            _unitOfWorkMock.GetBasketItemByCondition(DataGenerator.BasketItems[0]);

            var createBasketItemComand = new CreateBasketItemComand(It.IsAny<string>(), new(It.IsAny<string>()));
            var handler = new CreateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(createBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<AlreadyExistsException>()
                .WithMessage(BasketItemMessages.Exists);
        }

        [Fact]
        public async Task CreateBasketItem_InvalidUserId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);
            _unitOfWorkMock.GetItemByCondition(DataGenerator.Items[0]);

            var createBasketItemComand = new CreateBasketItemComand(It.IsAny<string>(), new(It.IsAny<string>()));
            var handler = new CreateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(createBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserMessages.NotFound);
        }

        [Fact]
        public async Task CreateBasketItem_InvalidItemId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetItemByCondition(null);

            var createBasketItemComand = new CreateBasketItemComand(It.IsAny<string>(), new(It.IsAny<string>()));
            var handler = new CreateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(createBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemMessages.NotFound);
        }

        [Fact]
        public async Task DeleteBasketItem_ValidParameters_ChangesBasketPrice()
        {
            //Arrange
            _unitOfWorkMock.DeleteBasketItem(DataGenerator.BasketItems[0]);

            var deleteBasketItemComand = new DeleteBasketItemComand(It.IsAny<string>(), It.IsAny<string>());
            var handler = new DeleteBasketItemHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(deleteBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteBasketItem_InvalidUserId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var deleteBasketItemComand = new DeleteBasketItemComand(It.IsAny<string>(), It.IsAny<string>());
            var handler = new DeleteBasketItemHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(deleteBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }

        [Fact]
        public async Task DeleteBasketItem_InvalidItemId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketItemWithItemByCondition(null);

            var deleteBasketItemComand = new DeleteBasketItemComand(It.IsAny<string>(), It.IsAny<string>());
            var handler = new DeleteBasketItemHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(deleteBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BasketItemMessages.NotFound);
        }


        [Fact]
        public async Task UpdateBasketItem_ValidParameters_ChangesItemCost()
        {
            //Arrange
            var basketItemMock = DataGenerator.BasketItems[0];

            _unitOfWorkMock.GetBasketItemWithItemByCondition(basketItemMock);

            var updateBasketItem = new UpdateBasketItemComand(It.IsAny<string>(), It.IsAny<string>(), 3);
            var handler = new UpdateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(updateBasketItem, cancellationToken: default);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateBasketItem_InvalidUserId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var updateBasketItem = new UpdateBasketItemComand(It.IsAny<string>(), It.IsAny<string>(), 3);
            var handler = new UpdateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(updateBasketItem, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }

        [Fact]
        public async Task UpdateBasketItem_InvalidItemId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketItemWithItemByCondition(null);

            var updateBasketItem = new UpdateBasketItemComand(It.IsAny<string>(), It.IsAny<string>(), 3);
            var handler = new UpdateBasketItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(updateBasketItem, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BasketItemMessages.NotFound);
        }
    }
}