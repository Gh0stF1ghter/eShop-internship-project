﻿using Catalogs.Application.Comands.ItemTypeCommands;
using Catalogs.Application.Handlers.ItemTypeHandlers;
using Catalogs.Application.Queries.ItemTypeQueries;
using Catalogs.Tests.FakeData;
using Catalogs.Tests.Mocks;

namespace Catalogs.Tests.HandlersTests
{
    public class ItemTypeHandlersTests
    {
        private readonly UnitOfWorkMock _unitOfWorkMock = new();
        private readonly CancellationToken _cancellationToken = It.IsAny<CancellationToken>();

        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc =>
                mc.AddProfile(new ItemTypeProfile())));

        public ItemTypeHandlersTests()
        {
            DataGenerator.InitBogusData();
        }

        [Fact]
        public async Task CreateItemTypeHandlerAsync_ValidParameters_ReturnsItemTypeDto()
        {
            //Arrange
            var itemTypeCreateDto = new ItemTypeManipulateDto("Foo");

            _unitOfWorkMock.IsItemTypeExists(false);

            var comand = new CreateItemTypeComand(itemTypeCreateDto);
            var handler = new CreateItemTypeHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(comand, _cancellationToken);

            //Assert
            response.Should()
                .BeOfType<ItemTypeDto>()
                .Which.Name.Should()
                .Be(itemTypeCreateDto.Name);
        }

        [Fact]
        public async Task CreateItemTypeHandlerAsync_ItemTypeExists_ThrowsBadRequestException()
        {
            //Arrange
            var itemTypeCreateDto = new ItemTypeManipulateDto("Foo");

            _unitOfWorkMock.IsItemTypeExists(true);

            var comand = new CreateItemTypeComand(itemTypeCreateDto);
            var handler = new CreateItemTypeHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(ItemTypeMessages.ItemTypeExists);
        }

        [Fact]
        public async Task DeleteItemTypeHandlerAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var itemType = DataGenerator.ItemTypes[0];

            _unitOfWorkMock.GetItemTypeById(itemType);

            var comand = new DeleteItemTypeComand(1, false);
            var handler = new DeleteItemTypeHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteItemTypeHandlerAsync_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetItemTypeById(null);

            var comand = new DeleteItemTypeComand(1, false);
            var handler = new DeleteItemTypeHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemTypeMessages.ItemTypeNotFound);
        }

        [Fact]
        public async Task GetItemTypeHandlerAsync_ValidParameters_ReturnsItemTypeDto()
        {
            //Arrange
            var itemType = DataGenerator.ItemTypes[0];

            _unitOfWorkMock.GetItemTypeById(itemType);

            var query = new GetItemTypeQuery(1, false);
            var handler = new GetItemTypeHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(query, _cancellationToken);

            //Assert
            response.Should()
                .BeOfType<ItemTypeDto>()
                .Which.Id.Should()
                .Be(itemType.Id);
        }

        [Fact]
        public async Task GetItemTypeHandlerAsync_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetItemTypeById(null);

            var query = new GetItemTypeQuery(1, false);
            var handler = new GetItemTypeHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(query, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemTypeMessages.ItemTypeNotFound);
        }

        [Fact]
        public async Task GetItemTypesHandlerAsync_ValidParameters_ReturnsItemTypeDtoList()
        {
            //Arrange
            _unitOfWorkMock.GetAllItemTypes();

            var query = new GetItemTypesQuery(false);
            var handler = new GetItemTypesHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(query, _cancellationToken);

            //Assert
            response.Should()
                .BeOfType<List<ItemTypeDto>>()
                .Which.Count.Should()
                .Be(2);
        }

        [Theory]
        [InlineData(0, "Loe")]
        [InlineData(1, "Doow")]
        public async Task UpdateItemTypeHandlerAsync_ValidParameters_ReturnsNoContent(int id, string newName)
        {
            //Arrange
            var itemType = DataGenerator.ItemTypes[0];

            var itemTypeUpdateDto = new ItemTypeManipulateDto(newName);

            _unitOfWorkMock.GetItemTypeById(itemType);

            var comand = new UpdateItemTypeComand(id, itemTypeUpdateDto, true);
            var handler = new UpdateItemTypeHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateItemTypeHandlerAsync_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            var itemTypeUpdateDto = new ItemTypeManipulateDto("Foo");

            _unitOfWorkMock.GetItemTypeById(null);

            var comand = new UpdateItemTypeComand(It.IsAny<int>(), itemTypeUpdateDto, true);
            var handler = new UpdateItemTypeHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemTypeMessages.ItemTypeNotFound);
        }
    }
}