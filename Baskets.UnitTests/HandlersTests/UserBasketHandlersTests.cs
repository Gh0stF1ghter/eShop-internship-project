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
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.UnitTests.HandlersTests
{
    public class UserBasketHandlersTests
    {
        private static CancellationToken _cancellationToken = It.IsAny<CancellationToken>();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc => mc.AddProfiles(
            [
                new UserBasketProfile()
            ])));

        [Theory]
        [InlineData("65e0f6b92fa24267a5c3fa13", 705)]
        [InlineData("65e0f6b02fa24267a5c3fa13", 0)]
        public async Task GetUserBasketReturnsUserBasket(string userId, double expectedTotalPrice)
        {
            var basket = FakeDb.Baskets.Where(b => b.UserId.Equals(userId)).First();

            _unitOfWork.Setup(uof => uof.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), _cancellationToken))
                .ReturnsAsync(basket);

            var query = new GetUserBasketQuery(userId);
            var handler = new GetUserBasketHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(query, cancellationToken: default);

            response.TotalPrice.Should()
                .Be(expectedTotalPrice);
        }

        [Theory]
        [InlineData("65e0f6b920a24267a5c3fa13")]
        [InlineData("35e0f6b02fa24267a5c3fa13")]
        public async Task GetUserBasketThrowsNotFoundException(string userId)
        {
            _unitOfWork.Setup(uof => uof.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), _cancellationToken))
                .ReturnsAsync((UserBasket)null);

            var query = new GetUserBasketQuery(userId);
            var handler = new GetUserBasketHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(query, cancellationToken: default);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);

        }

        [Fact]
        public async Task CreateUserBasketReturnsUserBasket()
        {
            _unitOfWork.Setup(uof => uof.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), _cancellationToken))
                .ReturnsAsync(FakeDb.Baskets.Where(b => b.UserId.Equals("65e0f6b920a24267a5c3fa13")).FirstOrDefault());

            _unitOfWork.Setup(uof => uof.User.GetByConditionAsync(It.IsAny<Expression<Func<User, bool>>>(), _cancellationToken))
                .ReturnsAsync(FakeDb.Users.Where(b => b.Id.Equals("65e0f6b920a24267a5c3fa13")).First());

            var comand = new CreateUserBasketComand("65e0f6b920a24267a5c3fa13");
            var handler = new CreateUserBasketHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(comand, cancellationToken: default);

            response.UserId.Should().Be("65e0f6b920a24267a5c3fa13");
        }

        [Fact]
        public async Task CreateUserBasketThrowsNotFoundException()
        {
            _unitOfWork.Setup(uof => uof.User.GetByConditionAsync(It.IsAny<Expression<Func<User, bool>>>(), _cancellationToken))
                .ReturnsAsync((User)null);

            var comand = new CreateUserBasketComand("sample");
            var handler = new CreateUserBasketHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(comand, cancellationToken: default);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserMessages.NotFound);
        }

        [Fact]
        public async Task CreateUserBasketThrowsAlreadyExistException()
        {
            _unitOfWork.Setup(uof => uof.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), _cancellationToken))
                .ReturnsAsync(FakeDb.Baskets.Where(b => b.UserId.Equals("65e0f6b92fa24267a5c3fa13")).First());
            _unitOfWork.Setup(uof => uof.User.GetByConditionAsync(It.IsAny<Expression<Func<User, bool>>>(), _cancellationToken))
                .ReturnsAsync(FakeDb.Users.Where(b => b.Id.Equals("65e0f6b92fa24267a5c3fa13")).First());

            var comand = new CreateUserBasketComand("65e0f6b92fa24267a5c3fa13");
            var handler = new CreateUserBasketHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(comand, cancellationToken: default);

            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(UserBasketMessages.Exists);
        }

        [Fact]
        public async Task DeleteUserBasketReturnsNoContent()
        {
            var basket = FakeDb.Baskets.Where(b => b.UserId.Equals("65e0f6b02fa24267a5c3fa13")).First();

            _unitOfWork.Setup(uof => uof.Basket.DeleteAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), _cancellationToken))
                .ReturnsAsync(basket);
            _unitOfWork.Setup(uof => uof.User.GetByConditionAsync(It.IsAny<Expression<Func<User, bool>>>(), _cancellationToken))
                .ReturnsAsync(FakeDb.Users.Where(b => b.Id.Equals("65e0f6b02fa24267a5c3fa13")).FirstOrDefault());

            var comand = new DeleteUserBasketComand("65e0f6b02fa24267a5c3fa13");
            var handler = new DeleteUserBasketHandler(_unitOfWork.Object);

            await handler.Handle(comand, cancellationToken: default);
        }

        [Fact]
        public async Task DeleteUserBasketThrowsBadRequestException()
        {
            _unitOfWork.Setup(uof => uof.User.GetByConditionAsync(It.IsAny<Expression<Func<User, bool>>>(), _cancellationToken))
                .ReturnsAsync(FakeDb.Users.Where(b => b.Id.Equals("65e0f6b92fa24267a5c3fa13")).First());

            var comand = new DeleteUserBasketComand("65e0f6b92fa24267a5c3fa13");
            var handler = new DeleteUserBasketHandler(_unitOfWork.Object);

            var response = async () => await handler.Handle(comand, cancellationToken: default);

            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(UserBasketMessages.UserExists);
        }

        [Fact]
        public async Task DeleteUserBasketThrowsNotFoundException()
        {
            _unitOfWork.Setup(uof => uof.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), _cancellationToken))
                .ReturnsAsync((UserBasket)null);
            _unitOfWork.Setup(uof => uof.User.GetByConditionAsync(It.IsAny<Expression<Func<User, bool>>>(), _cancellationToken))
                .ReturnsAsync((User)null);


            var comand = new DeleteUserBasketComand("sample");
            var handler = new DeleteUserBasketHandler(_unitOfWork.Object);

            var response = async () => await handler.Handle(comand, cancellationToken: default);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(UserBasketMessages.NotFound);
        }
    }
}
