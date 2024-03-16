using AutoMapper;
using Baskets.BusinessLogic.CQRS.Comands.UserBasketComands.CreateUserBasketComand;
using Baskets.BusinessLogic.CQRS.Comands.UserBasketComands.DeleteUserBasketComand;
using Baskets.BusinessLogic.CQRS.Queries.UserBasketQueries.GetUserBasketQuery;
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
    public class UserBasketHandlersTests
    {
        private readonly UnitOfWorkMock _unitOfWorkMock = new();
        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc => mc.AddProfiles(
            [
                new UserBasketProfile()
            ])));

        [Theory]
        [InlineData("65e0f6b92fa24267a5c3fa13", 705)]
        [InlineData("65e0f6b02fa24267a5c3fa13", 0)]
        public async Task GetUserBasket_ValidParameters_ReturnsUserBasket(string userId, double expectedTotalPrice)
        {
            //Arrange
            var basket = FakeDb.Baskets.Find(b => b.UserId.Equals(userId));

            _unitOfWorkMock.GetBasketByCondition(basket);

            var query = new GetUserBasketQuery(userId);
            var handler = new GetUserBasketHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(query, cancellationToken: default);

            response.TotalPrice.Should()
                .Be(expectedTotalPrice);
        }

        [Theory]
        [InlineData("65e0f6b920a24267a5c3fa13")]
        [InlineData("35e0f6b02fa24267a5c3fa13")]
        public async Task GetUserBasket_InvalidId_ThrowsNotFoundException(string userId)
        {
            //Arrange
            _unitOfWorkMock.GetBasketByCondition(null);

            var query = new GetUserBasketQuery(userId);
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
            _unitOfWorkMock.GetBasketByCondition(FakeDb.Baskets.Find(b => b.UserId.Equals("65e0f6b920a24267a5c3fa13")));

            _unitOfWorkMock.GetUserByCondition(FakeDb.Users.Find(b => b.Id.Equals("65e0f6b920a24267a5c3fa13")));

            var comand = new CreateUserBasketComand("65e0f6b920a24267a5c3fa13");
            var handler = new CreateUserBasketHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(comand, cancellationToken: default);

            response.UserId.Should().Be("65e0f6b920a24267a5c3fa13");
        }

        [Fact]
        public async Task CreateUserBasket_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetUserByCondition(null);

            var comand = new CreateUserBasketComand("sample");
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
            _unitOfWorkMock.GetBasketByCondition(FakeDb.Baskets.Find(b => b.UserId.Equals("65e0f6b92fa24267a5c3fa13")));
            _unitOfWorkMock.GetUserByCondition(FakeDb.Users.Find(b => b.Id.Equals("65e0f6b92fa24267a5c3fa13")));

            var comand = new CreateUserBasketComand("65e0f6b92fa24267a5c3fa13");
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
            var basket = FakeDb.Baskets.Find(b => b.UserId.Equals("65e0f6b02fa24267a5c3fa13"));

            _unitOfWorkMock.DeleteBasket(basket);
            _unitOfWorkMock.GetUserByCondition(FakeDb.Users.Find(b => b.Id.Equals("65e0f6b02fa24267a5c3fa13")));

            var comand = new DeleteUserBasketComand("65e0f6b02fa24267a5c3fa13");
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
            _unitOfWorkMock.GetUserByCondition(FakeDb.Users.Find(b => b.Id.Equals("65e0f6b92fa24267a5c3fa13")));

            var comand = new DeleteUserBasketComand("65e0f6b92fa24267a5c3fa13");
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


            var comand = new DeleteUserBasketComand("sample");
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
