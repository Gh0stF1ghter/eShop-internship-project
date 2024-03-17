﻿using Baskets.BusinessLogic.CQRS.Comands.UserBasketComands.CreateUserBasketComand;
using Baskets.BusinessLogic.CQRS.Comands.UserBasketComands.DeleteUserBasketComand;
using Baskets.BusinessLogic.CQRS.Queries.UserBasketQueries.GetUserBasketQuery;
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
        public async Task GetUserBasket_ValidParameters_ReturnsUserBasket()
        {
            //Arrange
            var basket = DataGenerator.UserBaskets[0];

            _unitOfWorkMock.GetBasketByCondition(basket);

            var query = new GetUserBasketQuery(It.IsAny<string>());
            var handler = new GetUserBasketHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(query, cancellationToken: default);

            response.Should()
                .BeOfType<UserBasketDto>();
        }

        [Fact]
        public async Task GetUserBasket_InvalidId_ThrowsNotFoundException()
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
        public async Task CreateUserBasket_ValidParameters_ReturnsUserBasket()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            _unitOfWorkMock.GetUserByCondition(DataGenerator.Users[0]);

            var comand = new CreateUserBasketComand(It.IsAny<string>());
            var handler = new CreateUserBasketHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(comand, cancellationToken: default);

            //Assert
            response.Should().BeOfType<UserBasketDto>();
        }

        [Fact]
        public async Task CreateUserBasket_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetUserByCondition(null);

            var comand = new CreateUserBasketComand(It.IsAny<string>());
            var handler = new CreateUserBasketHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserMessages.NotFound);
        }

        [Fact]
        public async Task CreateUserBasket_BasketExists_ThrowsAlreadyExistException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(DataGenerator.UserBaskets[0]);
            _unitOfWorkMock.GetUserByCondition(DataGenerator.Users[0]);

            var comand = new CreateUserBasketComand(It.IsAny<string>());
            var handler = new CreateUserBasketHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(UserBasketMessages.Exists);
        }

        [Fact]
        public async Task DeleteUserBasket_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var basket = DataGenerator.UserBaskets[0];

            _unitOfWorkMock.DeleteBasket(basket);
            _unitOfWorkMock.GetUserByCondition(null);

            var comand = new DeleteUserBasketComand(It.IsAny<string>());
            var handler = new DeleteUserBasketHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, cancellationToken: default);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteUserBasket_UserExists_ThrowsBadRequestException()
        {
            //Arrange
            _unitOfWorkMock.GetUserByCondition(DataGenerator.Users[0]);

            var comand = new DeleteUserBasketComand(It.IsAny<string>());
            var handler = new DeleteUserBasketHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, cancellationToken: default);

            //Assert
            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(UserBasketMessages.UserExists);
        }

        [Fact]
        public async Task DeleteUserBasket_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);
            _unitOfWorkMock.GetUserByCondition(null);


            var comand = new DeleteUserBasketComand(It.IsAny<string>());
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
