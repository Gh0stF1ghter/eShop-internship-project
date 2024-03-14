using AutoMapper;
using Catalogs.Application.Comands.BrandCommands;
using Catalogs.Application.Comands.ItemTypeCommands;
using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using Catalogs.Application.Handlers.BrandHandlers;
using Catalogs.Application.Handlers.ItemTypeHandlers;
using Catalogs.Application.MappingProfiles;
using Catalogs.Application.Queries.BrandQueries;
using Catalogs.Application.Queries.ItemTypeQueries;
using Catalogs.Domain.Entities.Constants;
using Catalogs.Domain.Entities.Exceptions;
using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Catalogs.Tests.HandlersTests
{
    public class ItemTypeHandlersTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CancellationToken _cancellationToken = It.IsAny<CancellationToken>();

        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc =>
                mc.AddProfile(new ItemTypeProfile())));

        [Fact]
        public async Task CreateItemTypeHandler_ValidParameters_ReturnsItemTypeDto()
        {
            var itemTypeCreateDto = new ItemTypeManipulateDto("Foo");

            _unitOfWork.Setup(uof => uof.ItemType.IsExistAsync(It.IsAny<Expression<Func<ItemType, bool>>>(), _cancellationToken))
                .ReturnsAsync(false);

            var comand = new CreateItemTypeComand(itemTypeCreateDto);
            var handler = new CreateItemTypeHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(comand, _cancellationToken);

            response.Should()
                .BeOfType<ItemTypeDto>()
                .Which.Name.Should()
                .Be(itemTypeCreateDto.Name);
        }

        [Fact]
        public async Task CreateItemTypeHandler_ItemTypeExists_ThrowsBadRequestException()
        {
            var itemTypeCreateDto = new ItemTypeManipulateDto("Foo");

            _unitOfWork.Setup(uof => uof.ItemType.IsExistAsync(It.IsAny<Expression<Func<ItemType, bool>>>(), _cancellationToken))
                .ReturnsAsync(true);

            var comand = new CreateItemTypeComand(itemTypeCreateDto);
            var handler = new CreateItemTypeHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(comand, _cancellationToken);

            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(ItemTypeMessages.ItemTypeExists);
        }

        [Fact]
        public async Task DeleteItemTypeHandler_ValidParameters_ReturnsNoContent()
        {
            var itemTypeList = new List<ItemType>
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
            };

            _unitOfWork.Setup(uof => uof.ItemType.GetItemTypeByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync(itemTypeList[0]);

            _unitOfWork.Setup(uof => uof.ItemType.Delete(itemTypeList[0]))
                .Callback(() => itemTypeList.Remove(itemTypeList[0]));

            var comand = new DeleteItemTypeComand(1, false);
            var handler = new DeleteItemTypeHandler(_unitOfWork.Object);

            await handler.Handle(comand, _cancellationToken);

            itemTypeList.Count.Should().Be(1);
        }

        [Fact]
        public async Task DeleteItemTypeHandler_InvalidId_ThrowsNotFoundException()
        {
            _unitOfWork.Setup(uof => uof.ItemType.GetItemTypeByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync((ItemType)null);

            var comand = new DeleteItemTypeComand(1, false);
            var handler = new DeleteItemTypeHandler(_unitOfWork.Object);

            var response = async () => await handler.Handle(comand, _cancellationToken);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemTypeMessages.ItemTypeNotFound);
        }

        [Fact]
        public async Task GetItemTypeHandler_ValidParameters_ReturnsItemTypeDto()
        {
            var itemTypeList = new List<ItemType>
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
            };

            _unitOfWork.Setup(uof => uof.ItemType.GetItemTypeByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync(itemTypeList[0]);

            var query = new GetItemTypeQuery(1, false);
            var handler = new GetItemTypeHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(query, _cancellationToken);

            response.Should()
                .BeOfType<ItemTypeDto>()
                .Which.Id.Should()
                .Be(1);
        }

        [Fact]
        public async Task GetItemTypeHandler_InvalidId_ThrowsNotFoundException()
        {
            _unitOfWork.Setup(uof => uof.ItemType.GetItemTypeByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync((ItemType)null);

            var query = new GetItemTypeQuery(1, false);
            var handler = new GetItemTypeHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(query, _cancellationToken);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemTypeMessages.ItemTypeNotFound);
        }

        [Fact]
        public async Task GetItemTypesHandler_ValidParameters_ReturnsItemTypeDtoList()
        {
            var itemTypeList = new List<ItemType>
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
            };

            _unitOfWork.Setup(uof => uof.ItemType.GetAllItemTypesAsync(false, _cancellationToken))
                .ReturnsAsync(itemTypeList);

            var query = new GetItemTypesQuery(false);
            var handler = new GetItemTypesHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(query, _cancellationToken);

            response.Should()
                .BeOfType<List<ItemTypeDto>>()
                .Which.Count.Should()
                .Be(2);
        }

        [Theory]
        [InlineData(0, "Loe")]
        [InlineData(1, "Doow")]
        public async Task UpdateItemTypeHandler_ValidParameters_ReturnsNoContent(int id, string newName)
        {
            var brandList = new List<ItemType>
            {
                new()
                {
                    Id = 0,
                    Name = "Foo",
                },
                new()
                {
                    Id = 1,
                    Name = "Boo",
                },
            };

            var itemTypeUpdateDto = new ItemTypeManipulateDto(newName);

            _unitOfWork.Setup(uof => uof.ItemType.GetItemTypeByIdAsync(id, true, _cancellationToken))
                .ReturnsAsync(brandList[id]);

            var comand = new UpdateItemTypeComand(id, itemTypeUpdateDto, true);
            var handler = new UpdateItemTypeHandler(_unitOfWork.Object, _mapper);

            await handler.Handle(comand, _cancellationToken);

            brandList[id].Should()
                .BeOfType<ItemType>()
                .Which.Name.Should()
                .Be(itemTypeUpdateDto.Name);
        }

        [Fact]
        public async Task UpdateItemTypeHandler_InvalidId_ThrowsNotFoundException()
        {

            var itemTypeUpdateDto = new ItemTypeManipulateDto("Foo");

            _unitOfWork.Setup(uof => uof.ItemType.GetItemTypeByIdAsync(It.IsAny<int>(), true, _cancellationToken))
                .ReturnsAsync((ItemType)null);

            var comand = new UpdateItemTypeComand(It.IsAny<int>(), itemTypeUpdateDto, true);
            var handler = new UpdateItemTypeHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(comand, _cancellationToken);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemTypeMessages.ItemTypeNotFound);
        }
    }
}