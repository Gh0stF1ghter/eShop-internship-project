using AutoMapper;
using Catalogs.Application.Comands.ItemCommands;
using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using Catalogs.Application.Handlers.ItemHandlers;
using Catalogs.Application.MappingProfiles;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.Constants;
using Catalogs.Domain.Entities.Exceptions;
using Catalogs.Domain.Entities.LinkModels;
using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces;
using Catalogs.Domain.RequestFeatures;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;

namespace Catalogs.Tests.HandlersTests
{
    public class ItemHandlersTests
    {
        private static CancellationToken _cancelationToken = It.IsAny<CancellationToken>();

        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IItemLinks<ItemDto>> _itemLinks = new();
        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc =>
            mc.AddProfile(new ItemProfile())));

        public ItemHandlersTests()
        {
            _unitOfWork.Setup(uof => uof.ItemType.IsExistAsync(It.IsAny<Expression<Func<ItemType, bool>>>(), _cancelationToken))
                .ReturnsAsync(true);
            _unitOfWork.Setup(uof => uof.Brand.IsExistAsync(It.IsAny<Expression<Func<Brand, bool>>>(), _cancelationToken))
                .ReturnsAsync(true);
            _unitOfWork.Setup(uof => uof.Vendor.IsExistAsync(It.IsAny<Expression<Func<Vendor, bool>>>(), _cancelationToken))
                .ReturnsAsync(true);
        }

        [Fact]
        public async Task DeleteItemHandler_ValidParameters_ReturnsNoContent()
        {
            _unitOfWork.Setup(uof => uof.Item.GetItemOfTypeByIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), _cancelationToken))
                .ReturnsAsync(new Item()
                {
                    Id = 1,
                    Name = "Foo"
                });

            _unitOfWork.Setup(uof => uof.Item.Delete(It.IsAny<Item>()));

            var comand = new DeleteItemComand(1, 1, false);
            var handle = new DeleteItemHandler(_unitOfWork.Object);

            await handle.Handle(comand, token: default);
        }

        [Fact]
        public async Task DeleteItemHandler_InvalidId_ThrowsNotFoundException()
        {
            _unitOfWork.Setup(uof => uof.Item.GetItemOfTypeByIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), _cancelationToken))
            .ReturnsAsync((Item)null);

            var comand = new DeleteItemComand(1, 1, false);
            var handle = new DeleteItemHandler(_unitOfWork.Object);

            var response = async () => await handle.Handle(comand, token: default);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemMessages.ItemNotFound);
        }

        [Fact]
        public async Task CreateItemHandler_ValidParameters_ReturnsItem()
        {
            var itemCreateDto = new ItemManipulateDto("foo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWork.Setup(uof => uof.Item.IsExistAsync(It.IsAny<Expression<Func<Item, bool>>>(), _cancelationToken))
                .ReturnsAsync(false);

            var comand = new CreateItemComand(itemCreateDto, 1, false);
            var handle = new CreateItemHandler(_unitOfWork.Object, _mapper);

            var response = await handle.Handle(comand, token: default);

            response.Should()
                .BeOfType<ItemDto>()
                .Which.Name.Should()
                .Be("foo");
        }

        [Theory]
        [InlineData(false, false, true, ItemTypeMessages.ItemTypeNotFound)]
        [InlineData(true, false, false, BrandMessages.BrandNotFound)]
        [InlineData(true, true, false, VendorMessages.VendorNotFound)]
        public async Task CreateItemHandler_InvalidId_ThrowsNotFoundException(bool typeExists, bool brandExists, bool vendorExists, string expectedMessage)
        {
            var itemCreateDto = new ItemManipulateDto("foo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWork.Setup(uof => uof.Item.IsExistAsync(It.IsAny<Expression<Func<Item, bool>>>(), _cancelationToken))
                .ReturnsAsync(false);

            _unitOfWork.Setup(uof => uof.ItemType.IsExistAsync(It.IsAny<Expression<Func<ItemType, bool>>>(), _cancelationToken))
                .ReturnsAsync(typeExists);
            _unitOfWork.Setup(uof => uof.Brand.IsExistAsync(It.IsAny<Expression<Func<Brand, bool>>>(), _cancelationToken))
                .ReturnsAsync(brandExists);
            _unitOfWork.Setup(uof => uof.Vendor.IsExistAsync(It.IsAny<Expression<Func<Vendor, bool>>>(), _cancelationToken))
                .ReturnsAsync(vendorExists);

            var comand = new CreateItemComand(itemCreateDto, 1, false);
            var handle = new CreateItemHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handle.Handle(comand, token: default);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(expectedMessage);
        }

        [Fact]
        public async Task CreateItemHandler_InvalidItemId_ThrowsBadRequestException()
        {
            var itemCreateDto = new ItemManipulateDto("foo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWork.Setup(uof => uof.Item.IsExistAsync(It.IsAny<Expression<Func<Item, bool>>>(), _cancelationToken))
                .ReturnsAsync(true);

            var comand = new CreateItemComand(itemCreateDto, 1, false);
            var handle = new CreateItemHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handle.Handle(comand, token: default);

            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(ItemMessages.ItemExists);
        }

        [Fact]
        public async Task GetItemOfTypeHandler_ValidParameters_ReturnsItem()
        {
            var item = new Item()
            {
                Id = 1,
                Name = "Foo"
            };

            _unitOfWork.Setup(uof => uof.Item.GetItemOfTypeByIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), _cancelationToken)).
                ReturnsAsync(item);

            var query = new GetItemOfTypeQuery(1, 1, false);
            var handle = new GetItemOfTypeHandler(_unitOfWork.Object, _mapper);

            var response = await handle.Handle(query, token: default);

            response.Should()
                .BeOfType<ItemDto>()
                .Which.Name.Should()
                .Be(item.Name);
        }

        [Theory]
        [InlineData(false, ItemTypeMessages.ItemTypeNotFound)]
        [InlineData(true, ItemMessages.ItemNotFound)]
        public async Task GetItemOfTypeHandler_InvalidId_ThrowsNotFoundException(bool typeExist, string expectedMessage)
        {
            _unitOfWork.Setup(uof => uof.ItemType.IsExistAsync(It.IsAny<Expression<Func<ItemType, bool>>>(), _cancelationToken))
                .ReturnsAsync(typeExist);

            _unitOfWork.Setup(uof => uof.Item.GetItemOfTypeByIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), _cancelationToken))
                .ReturnsAsync((Item)null);

            var query = new GetItemOfTypeQuery(1, 1, false);
            var handle = new GetItemOfTypeHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handle.Handle(query, token: default);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(expectedMessage);
        }

        [Fact]
        public async Task GetItemsOfTypeHandler_ValidParameters_ReturnsListOfItems()
        {
            var itemList = new PagedList<Item>(
                [
                    new()
                    {
                        Id = 1,
                        Name = "Foo"
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Boo"
                    }
                ], 1, 1, 5);

            var linkParameters = new LinkParameters(new ItemParameters(), It.IsAny<HttpContext>());

            _unitOfWork.Setup(uof => uof.Item.GetAllItemsOfTypeAsync(It.IsAny<int>(), It.IsAny<ItemParameters>(), It.IsAny<bool>(), _cancelationToken)).
                ReturnsAsync(itemList);

            _itemLinks.Setup(il => il.TryGenerateLinks(It.IsAny<IEnumerable<ItemDto>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<HttpContext>()))
                .Returns(It.IsAny<LinkResponse>());

            var query = new GetItemsOfTypeQuery(1, linkParameters, false);
            var handle = new GetItemsOfTypeHandler(_unitOfWork.Object, _mapper, _itemLinks.Object);

            var response = await handle.Handle(query, token: default);

            response.Should()
                .BeOfType<(LinkResponse, MetaData)>();
        }

        [Fact]
        public async Task GetItemsOfTypeHandler_InvalidTypeId_ThrowsNotFoundException()
        {
            _unitOfWork.Setup(uof => uof.ItemType.IsExistAsync(It.IsAny<Expression<Func<ItemType, bool>>>(), _cancelationToken))
                .ReturnsAsync(false);

            var query = new GetItemsOfTypeQuery(1, It.IsAny<LinkParameters>(), false);
            var handle = new GetItemsOfTypeHandler(_unitOfWork.Object, _mapper, _itemLinks.Object);

            var response = async () => await handle.Handle(query, token: default);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(ItemTypeMessages.ItemTypeNotFound);
        }

        [Fact]
        public async Task UpdateItemHandler_ValidParameters_ReturnsNoContent()
        {
            var item = new Item()
            {
                Id = 1,
                Name = "Foo"
            };

            var itemUpdateDto = new ItemManipulateDto("boo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWork.Setup(uof => uof.Item.GetItemOfTypeByIdAsync(It.IsAny<int>(), It.IsAny<int>(), true, _cancelationToken))
                .ReturnsAsync(item);

            var comand = new UpdateItemComand(1, 1, itemUpdateDto, true);
            var handle = new UpdateItemHandler(_unitOfWork.Object, _mapper);

            await handle.Handle(comand, token: default);

            item.Name.Should().Be(itemUpdateDto.Name);
        }

        [Theory]
        [InlineData(false, false, true, ItemTypeMessages.ItemTypeNotFound)]
        [InlineData(true, false, false, BrandMessages.BrandNotFound)]
        [InlineData(true, true, false, VendorMessages.VendorNotFound)]
        public async Task UpdateItemHandler_InvalidId_ThrowsNotFoundException(bool typeExists, bool brandExists, bool vendorExists, string expectedMessage)
        {
            var itemCreateDto = new ItemManipulateDto("foo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWork.Setup(uof => uof.ItemType.IsExistAsync(It.IsAny<Expression<Func<ItemType, bool>>>(), _cancelationToken))
                .ReturnsAsync(typeExists);
            _unitOfWork.Setup(uof => uof.Brand.IsExistAsync(It.IsAny<Expression<Func<Brand, bool>>>(), _cancelationToken))
                .ReturnsAsync(brandExists);
            _unitOfWork.Setup(uof => uof.Vendor.IsExistAsync(It.IsAny<Expression<Func<Vendor, bool>>>(), _cancelationToken))
                .ReturnsAsync(vendorExists);

            var comand = new UpdateItemComand(1, 1, itemCreateDto, true);
            var handle = new UpdateItemHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handle.Handle(comand, token: default);

            await response.Should().ThrowAsync<NotFoundException>()
                 .WithMessage(expectedMessage);
        }

        [Fact]
        public async Task UpdateItemHandler_NullItemToUpdate_ThrowsBadRequestException()
        {
            var itemCreateDto = new ItemManipulateDto("foo", It.IsAny<int>(), 0, "2", It.IsAny<int>(), It.IsAny<int>());

            _unitOfWork.Setup(uof => uof.Item.GetItemOfTypeByIdAsync(It.IsAny<int>(), It.IsAny<int>(), true, _cancelationToken))
                .ReturnsAsync((Item)null);

            var comand = new UpdateItemComand(1, 1, itemCreateDto, true);
            var handle = new UpdateItemHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handle.Handle(comand, token: default);

            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(ItemMessages.ItemNotFound);
        }
    }
}
