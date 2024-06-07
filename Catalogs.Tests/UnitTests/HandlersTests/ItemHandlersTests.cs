using Catalogs.Application.CQRS.Commands.ItemCommands.CreateItem;
using Catalogs.Application.CQRS.Commands.ItemCommands.DeleteItem;
using Catalogs.Application.CQRS.Commands.ItemCommands.UpdateItem;
using Catalogs.Application.CQRS.Queries.ItemQueries.GetItemOfType;
using Catalogs.Application.CQRS.Queries.ItemQueries.GetItemsOfType;
using Catalogs.Domain.Entities.Constants.Messages;
using Catalogs.Domain.Entities.LinkModels;
using Catalogs.Domain.RequestFeatures;
using Catalogs.Tests.UnitTests.FakeDataGenerator;
using Catalogs.Tests.UnitTests.Mocks;
using Microsoft.AspNetCore.Http;

namespace Catalogs.Tests.UnitTests.HandlersTests
{
    public class ItemHandlersTests
    {
        private readonly UnitOfWorkMock _unitOfWorkMock = new();
        private readonly Mock<IItemLinks<ItemDto>> _itemLinks = new();

        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc =>
            mc.AddProfile(new ItemProfile())));

        public ItemHandlersTests()
        {
            DataGenerator.InitBogusData();

            _unitOfWorkMock.IsBrandExists(true);
            _unitOfWorkMock.IsItemTypeExists(true);
            _unitOfWorkMock.IsVendorExists(true);

            _itemLinks.Setup(il => il.TryGenerateLinks(It.IsAny<IEnumerable<ItemDto>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<HttpContext>()))
                .Returns(It.IsAny<LinkResponse>());
        }

        [Fact]
        public async Task DeleteItemHandlerAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var item = DataGenerator.Items[0];

            _unitOfWorkMock.GetItemById(item);

            var comand = new DeleteItemCommand(1, 1, false);
            var handle = new DeleteItemHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handle.Handle(comand, token: default);

            //Assert
            await response.Should()
                    .NotThrowAsync();
        }

        [Fact]
        public async Task DeleteItemHandlerAsync_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetItemById(null);

            var comand = new DeleteItemCommand(1, 1, false);
            var handle = new DeleteItemHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handle.Handle(comand, token: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemMessages.ItemNotFound);
        }

        [Fact]
        public async Task CreateItemHandlerAsync_ValidParameters_ReturnsItem()
        {
            //Arrange
            var itemCreateDto = new ItemManipulateDto("foo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWorkMock.IsItemExists(false);

            var comand = new CreateItemCommand(itemCreateDto, 1, false);
            var handle = new CreateItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handle.Handle(comand, token: default);

            //Assert
            response.Should()
                .BeOfType<ItemDto>()
                .Which.Name.Should()
                .Be("foo");
        }

        [Theory]
        [InlineData(false, false, true, ItemTypeMessages.ItemTypeNotFound)]
        [InlineData(true, false, false, BrandMessages.BrandNotFound)]
        [InlineData(true, true, false, VendorMessages.VendorNotFound)]
        public async Task CreateItemHandlerAsync_InvalidId_ThrowsNotFoundException(bool typeExists, bool brandExists, bool vendorExists, string expectedMessage)
        {
            //Arrange
            var itemCreateDto = new ItemManipulateDto("foo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWorkMock.IsItemExists(false);

            _unitOfWorkMock.IsItemTypeExists(typeExists);
            _unitOfWorkMock.IsBrandExists(brandExists);
            _unitOfWorkMock.IsVendorExists(vendorExists);

            var comand = new CreateItemCommand(itemCreateDto, 1, false);
            var handle = new CreateItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handle.Handle(comand, token: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(expectedMessage);
        }

        [Fact]
        public async Task CreateItemHandlerAsync_InvalidItemId_ThrowsBadRequestException()
        {
            //Arrange
            var itemCreateDto = new ItemManipulateDto("foo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWorkMock.IsItemExists(true);

            var comand = new CreateItemCommand(itemCreateDto, 1, false);
            var handle = new CreateItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handle.Handle(comand, token: default);

            //Assert
            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(ItemMessages.ItemExists);
        }

        [Fact]
        public async Task GetItemOfTypeHandlerAsync_ValidParameters_ReturnsItem()
        {
            //Arrange
            var item = DataGenerator.Items[0];

            _unitOfWorkMock.GetItemById(item);

            var query = new GetItemOfTypeQuery(1, 1, false);
            var handle = new GetItemOfTypeHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handle.Handle(query, token: default);

            //Assert
            response.Should()
                .BeOfType<ItemDto>()
                .Which.Name.Should()
                .Be(item.Name);
        }

        [Theory]
        [InlineData(false, ItemTypeMessages.ItemTypeNotFound)]
        [InlineData(true, ItemMessages.ItemNotFound)]
        public async Task GetItemOfTypeHandlerAsync_InvalidId_ThrowsNotFoundException(bool typeExist, string expectedMessage)
        {
            //Arrange
            _unitOfWorkMock.IsItemTypeExists(typeExist);
            _unitOfWorkMock.GetItemById(null);

            var query = new GetItemOfTypeQuery(1, 1, false);
            var handle = new GetItemOfTypeHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handle.Handle(query, token: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(expectedMessage);
        }

        [Fact]
        public async Task GetItemsOfTypeHandlerAsync_ValidParameters_ReturnsListOfItems()
        {
            //Arrange
            var itemList = DataGenerator.Items;

            var linkParameters = new LinkParameters(new ItemParameters(), It.IsAny<HttpContext>());

            _unitOfWorkMock.GetAllItems(itemList);

            var query = new GetItemsOfTypeQuery(1, linkParameters, false);
            var handle = new GetItemsOfTypeHandler(_unitOfWorkMock.Object, _mapper, _itemLinks.Object);

            //Act
            var response = await handle.Handle(query, token: default);

            //Assert
            response.Should()
                .BeOfType<(LinkResponse, MetaData)>();
        }

        [Fact]
        public async Task GetItemsOfTypeHandlerAsync_InvalidTypeId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.IsItemTypeExists(false);

            var query = new GetItemsOfTypeQuery(1, It.IsAny<LinkParameters>(), false);
            var handle = new GetItemsOfTypeHandler(_unitOfWorkMock.Object, _mapper, _itemLinks.Object);

            //Act
            var response = async () => await handle.Handle(query, token: default);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemTypeMessages.ItemTypeNotFound);
        }

        [Fact]
        public async Task UpdateItemHandlerAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var item = DataGenerator.Items[0];

            var itemUpdateDto = new ItemManipulateDto("boo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWorkMock.GetItemById(item);

            var comand = new UpdateItemCommand(1, 1, itemUpdateDto, true);
            var handle = new UpdateItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            await handle.Handle(comand, token: default);

            //Assert
            item.Name.Should().Be(itemUpdateDto.Name);
        }

        [Theory]
        [InlineData(false, false, true, ItemTypeMessages.ItemTypeNotFound)]
        [InlineData(true, false, false, BrandMessages.BrandNotFound)]
        [InlineData(true, true, false, VendorMessages.VendorNotFound)]
        public async Task UpdateItemHandlerAsync_InvalidId_ThrowsNotFoundException(bool typeExists, bool brandExists, bool vendorExists, string expectedMessage)
        {
            //Arrange
            var itemCreateDto = new ItemManipulateDto("foo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWorkMock.IsItemTypeExists(typeExists);
            _unitOfWorkMock.IsBrandExists(brandExists);
            _unitOfWorkMock.IsVendorExists(vendorExists);

            var comand = new UpdateItemCommand(1, 1, itemCreateDto, true);
            var handle = new UpdateItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handle.Handle(comand, token: default);

            //Assert
            await response.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(expectedMessage);
        }

        [Fact]
        public async Task UpdateItemHandlerAsync_NullItemToUpdate_ThrowsBadRequestException()
        {
            //Arrange
            var itemCreateDto = new ItemManipulateDto("foo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWorkMock.GetItemById(null);

            var comand = new UpdateItemCommand(1, 1, itemCreateDto, true);
            var handle = new UpdateItemHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handle.Handle(comand, token: default);

            //Assert
            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(ItemMessages.ItemNotFound);
        }
    }
}