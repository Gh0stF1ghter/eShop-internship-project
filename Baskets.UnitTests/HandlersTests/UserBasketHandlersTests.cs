using Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.CreateUserBasket;
using Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.DeleteUserBasket;
using Baskets.BusinessLogic.CQRS.Queries.UserBasketQueries.GetUserBasket;
using Baskets.BusinessLogic.DataTransferObjects;

namespace Baskets.UnitTests.HandlersTests
{
    public class UserBasketHandlersTests
    {
        private readonly UnitOfWorkMock _unitOfWorkMock = new();
        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc => mc.AddProfile(
                new UserBasketProfile())));

        public UserBasketHandlersTests()
        {
            DataGenerator.InitBogusData();
        }

        [Fact]
        public async Task GetUserBasketAsync_ValidParameters_ReturnsUserBasket()
        {
            //Arrange
            var basket = DataGenerator.UserBaskets[0];

            _unitOfWorkMock.GetBasketByCondition(basket);

            var query = new GetUserBasketQuery(It.IsAny<string>());
            var handler = new GetUserBasketHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(query, cancellationToken: default);

            //Assert
            response.Should()
                .BeOfType<UserBasketDto>();
        }

        [Fact]
        public async Task GetUserBasketAsync_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var query = new GetUserBasketQuery(It.IsAny<string>());
            var handler = new GetUserBasketHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(query, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }

        [Fact]
        public async Task CreateUserBasketAsync_ValidParameters_ReturnsUserBasket()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var comand = new CreateUserBasketCommand(It.IsAny<string>());
            var handler = new CreateUserBasketHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(comand, cancellationToken: default);

            //Assert
            response.Should().BeOfType<UserBasketDto>();
        }

        [Fact]
        public async Task CreateUserBasketAsync_BasketExists_ThrowsAlreadyExistException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(DataGenerator.UserBaskets[0]);

            var comand = new CreateUserBasketCommand(It.IsAny<string>());
            var handler = new CreateUserBasketHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(UserBasketMessages.Exists);
        }

        [Fact]
        public async Task DeleteUserBasketAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var basket = DataGenerator.UserBaskets[0];

            _unitOfWorkMock.DeleteBasket(basket);

            var comand = new DeleteUserBasketCommand(It.IsAny<string>());
            var handler = new DeleteUserBasketHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, cancellationToken: default);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteUserBasketAsync_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var comand = new DeleteUserBasketCommand(It.IsAny<string>());
            var handler = new DeleteUserBasketHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }
    }
}