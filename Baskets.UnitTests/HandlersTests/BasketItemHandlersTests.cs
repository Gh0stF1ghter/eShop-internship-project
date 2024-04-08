using Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.CreateBasketItem;
using Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.DeleteBasketItem;
using Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.UpdateBasketItem;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItem;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItems;

namespace Baskets.UnitTests.HandlersTests
{
    public class BasketItemHandlersTests
    {
        private readonly UnitOfWorkMock _unitOfWorkMock = new();
        private readonly ItemGrpcServiceClientMock _clientMock = new();

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
            _clientMock.GetItem(
                new()
                { 
                    Item = new ItemGrpcService.Item()
                    {
                        Id = 2,
                        BrandId = 4,
                        VendorId = 1,
                        TypeId = 1,
                        Name = "Book",
                        Price = 19.99,
                        ImageUrl = "book.jpg"
                    }
                });
        }

        [Fact]
        public async Task GetBasketItemsAsync_ValidParameters_ReturnsList()
        {
            //Arrange
            var getBasketItemsQuery = new GetBasketItemsQuery(It.IsAny<string>());
            var handler = new GetBasketItemsHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = await handler.Handle(getBasketItemsQuery, cancellationToken: default);

            //Assert
            response.Should().HaveCount(DataGenerator.BasketItems.Count);
        }

        [Fact]
        public async Task GetBasketItemsAsync_InvalidBasketId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var getBasketItemsQuery = new GetBasketItemsQuery(It.IsAny<string>());
            var handler = new GetBasketItemsHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(getBasketItemsQuery, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }

        [Fact]
        public async Task GetBasketItemAsync_ValidParameters_ReturnsBasketItem()
        {
            //Arrange
            var mockedResult = DataGenerator.BasketItems[0];

            _unitOfWorkMock.GetBasketItemWithItemByCondition(mockedResult);

            var getBasketItemQuery = new GetBasketItemQuery(It.IsAny<string>(), It.IsAny<string>());
            var handler = new GetBasketItemHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = await handler.Handle(getBasketItemQuery, cancellationToken: default);

            //Assert
            response.BasketItemId.Should()
                .BeEquivalentTo(mockedResult.BasketItemId);
        }

        [Fact]
        public async Task GetBasketItemAsync_InvalidBasketId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var getBasketItemQuery = new GetBasketItemQuery(It.IsAny<string>(), It.IsAny<string>());
            var handler = new GetBasketItemHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(getBasketItemQuery, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }

        [Fact]
        public async Task GetBasketItemAsync_InvalidBasketItemId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketItemWithItemByCondition(null);

            var getBasketItemQuery = new GetBasketItemQuery(It.IsAny<string>(), It.IsAny<string>());
            var handler = new GetBasketItemHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(getBasketItemQuery, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BasketItemMessages.NotFound);
        }

        [Fact]
        public async Task CreateBasketItemAsync_ValidParameters_ReturnsBasketItem()
        {
            //Arrange
            _unitOfWorkMock.GetBasketItemWithItemByCondition(null);

            var createBasketItemComand = new CreateBasketItemCommand(It.IsAny<string>(), new(It.IsAny<int>()));
            var handler = new CreateBasketItemHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = await handler.Handle(createBasketItemComand, cancellationToken: default);

            //Assert
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateBasketItemAsync_ExistingItemId_ThrowsAlreadyExistsException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketItemByCondition(DataGenerator.BasketItems[0]);

            var createBasketItemComand = new CreateBasketItemCommand(It.IsAny<string>(), new(It.IsAny<int>()));
            var handler = new CreateBasketItemHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(createBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<AlreadyExistsException>()
                .WithMessage(BasketItemMessages.Exists);
        }

        [Fact]
        public async Task CreateBasketItemAsync_InvalidUserId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var createBasketItemComand = new CreateBasketItemCommand(It.IsAny<string>(), new(It.IsAny<int>()));
            var handler = new CreateBasketItemHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(createBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserMessages.NotFound);
        }

        [Fact]
        public async Task DeleteBasketItemAsync_ValidParameters_ChangesBasketPrice()
        {
            //Arrange
            _unitOfWorkMock.DeleteBasketItem(DataGenerator.BasketItems[0]);

            var deleteBasketItemComand = new DeleteBasketItemCommand(It.IsAny<string>(), It.IsAny<string>());
            var handler = new DeleteBasketItemHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(deleteBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteBasketItemAsync_InvalidUserId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var deleteBasketItemComand = new DeleteBasketItemCommand(It.IsAny<string>(), It.IsAny<string>());
            var handler = new DeleteBasketItemHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(deleteBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }

        [Fact]
        public async Task DeleteBasketItemAsync_InvalidItemId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketItemWithItemByCondition(null);

            var deleteBasketItemComand = new DeleteBasketItemCommand(It.IsAny<string>(), It.IsAny<string>());
            var handler = new DeleteBasketItemHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(deleteBasketItemComand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BasketItemMessages.NotFound);
        }


        [Fact]
        public async Task UpdateBasketItemAsync_ValidParameters_ChangesItemCost()
        {
            //Arrange
            var basketItemMock = DataGenerator.BasketItems[0];

            _unitOfWorkMock.GetBasketItemWithItemByCondition(basketItemMock);

            var updateBasketItem = new UpdateBasketItemCommand(It.IsAny<string>(), It.IsAny<string>(), 3);
            var handler = new UpdateBasketItemHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(updateBasketItem, cancellationToken: default);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateBasketItemAsync_InvalidUserId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var updateBasketItem = new UpdateBasketItemCommand(It.IsAny<string>(), It.IsAny<string>(), 3);
            var handler = new UpdateBasketItemHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(updateBasketItem, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }

        [Fact]
        public async Task UpdateBasketItemAsync_InvalidItemId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketItemWithItemByCondition(null);

            var updateBasketItem = new UpdateBasketItemCommand(It.IsAny<string>(), It.IsAny<string>(), 3);
            var handler = new UpdateBasketItemHandler(_unitOfWorkMock.Object, _clientMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(updateBasketItem, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BasketItemMessages.NotFound);
        }
    }
}