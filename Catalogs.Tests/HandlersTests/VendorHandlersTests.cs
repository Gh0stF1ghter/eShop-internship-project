using AutoMapper;
using Catalogs.Application.Comands.ItemTypeCommands;
using Catalogs.Application.Comands.VendorCommands;
using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using Catalogs.Application.Handlers.ItemTypeHandlers;
using Catalogs.Application.Handlers.VendorHandlers;
using Catalogs.Application.MappingProfiles;
using Catalogs.Application.Queries.ItemTypeQueries;
using Catalogs.Application.Queries.VendorQueries;
using Catalogs.Domain.Entities.Constants;
using Catalogs.Domain.Entities.Exceptions;
using Catalogs.Domain.Entities.Models;
using Catalogs.Domain.Interfaces;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace Catalogs.Tests.HandlersTests
{
    public class VendorHandlersTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CancellationToken _cancellationToken = It.IsAny<CancellationToken>();

        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc =>
                mc.AddProfile(new VendorProfile())));

        [Fact]
        public async Task CreateVendorHandler_ValidParameters_ReturnsVendorDto()
        {
            var vendorCreateDto = new VendorManipulateDto("Foo");

            _unitOfWork.Setup(uof => uof.Vendor.IsExistAsync(It.IsAny<Expression<Func<Vendor, bool>>>(), _cancellationToken))
            .ReturnsAsync(false);

            var comand = new CreateVendorComand(vendorCreateDto);
            var handler = new CreateVendorHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(comand, _cancellationToken);

            response.Should()
                .BeOfType<VendorDto>()
                .Which.Name.Should()
                .Be(vendorCreateDto.Name);
        }

        [Fact]
        public async Task CreateVendorHandler_VendorExists_ThrowsBadRequestException()
        {
            var vendorCreateDto = new VendorManipulateDto("Foo");

            _unitOfWork.Setup(uof => uof.Vendor.IsExistAsync(It.IsAny<Expression<Func<Vendor, bool>>>(), _cancellationToken))
                .ReturnsAsync(true);

            var comand = new CreateVendorComand(vendorCreateDto);
            var handler = new CreateVendorHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(comand, _cancellationToken);

            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(VendorMessages.VendorExists);
        }

        [Fact]
        public async Task DeleteVendorHandler_ValidParameters_ReturnsNoContent()
        {
            var vendorList = new List<Vendor>
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

            _unitOfWork.Setup(uof => uof.Vendor.GetVendorByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync(vendorList[0]);

            _unitOfWork.Setup(uof => uof.Vendor.Delete(vendorList[0]))
                .Callback(() => vendorList.Remove(vendorList[0]));

            var comand = new DeleteVendorComand(1, false);
            var handler = new DeleteVendorHandler(_unitOfWork.Object);

            await handler.Handle(comand, _cancellationToken);

            vendorList.Count.Should().Be(1);
        }

        [Fact]
        public async Task DeleteVendorHandler_InvalidId_ThrowsNotFoundException()
        {
            _unitOfWork.Setup(uof => uof.Vendor.GetVendorByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync((Vendor)null);

            var comand = new DeleteVendorComand(1, false);
            var handler = new DeleteVendorHandler(_unitOfWork.Object);

            var response = async () => await handler.Handle(comand, _cancellationToken);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(VendorMessages.VendorNotFound);
        }

        [Fact]
        public async Task GetVendorHandler_ValidParameters_ReturnsVendorDto()
        {
            var vendorList = new List<Vendor>
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

            _unitOfWork.Setup(uof => uof.Vendor.GetVendorByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync(vendorList[0]);

            var query = new GetVendorQuery(1, false);
            var handler = new GetVendorHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(query, _cancellationToken);

            response.Should()
                .BeOfType<VendorDto>()
                .Which.Id.Should()
                .Be(1);
        }

        [Fact]
        public async Task GetVendorHandler_InvalidId_ThrowsNotFoundException()
        {
            _unitOfWork.Setup(uof => uof.Vendor.GetVendorByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync((Vendor)null);

            var query = new GetVendorQuery(1, false);
            var handler = new GetVendorHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(query, _cancellationToken);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(VendorMessages.VendorNotFound);
        }

        [Fact]
        public async Task GetVendorHandler_ValidParameters_ReturnsVendorDtoList()
        {
            var vendorList = new List<Vendor>
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

            _unitOfWork.Setup(uof => uof.Vendor.GetAllVendorsAsync(false, _cancellationToken))
                .ReturnsAsync(vendorList);

            var query = new GetVendorsQuery(false);
            var handler = new GetVendorsHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(query, _cancellationToken);

            response.Should()
                .BeOfType<List<VendorDto>>()
                .Which.Count.Should()
                .Be(2);
        }

        [Theory]
        [InlineData(0, "Loe")]
        [InlineData(1, "Doow")]
        public async Task UpdateVendorHandler_ValidParameters_ReturnsNoContent(int id, string newName)
        {
            var vendorList = new List<Vendor>
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

            var endorUpdateDto = new VendorManipulateDto(newName);

            _unitOfWork.Setup(uof => uof.Vendor.GetVendorByIdAsync(id, true, _cancellationToken))
                .ReturnsAsync(vendorList[id]);

            var comand = new UpdateVendorComand(id, endorUpdateDto, true);
            var handler = new UpdateVendorHandler(_unitOfWork.Object, _mapper);

            await handler.Handle(comand, _cancellationToken);

            vendorList[id].Should()
                .BeOfType<Vendor>()
                .Which.Name.Should()
                .Be(endorUpdateDto.Name);
        }

        [Fact]
        public async Task UpdateVendorHandler_InvalidId_ThrowsNotFoundException()
        {

            var vendorUpdateDto = new VendorManipulateDto("Foo");

            _unitOfWork.Setup(uof => uof.Vendor.GetVendorByIdAsync(It.IsAny<int>(), true, _cancellationToken))
                .ReturnsAsync((Vendor)null);

            var comand = new UpdateVendorComand(It.IsAny<int>(), vendorUpdateDto, true);
            var handler = new UpdateVendorHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(comand, _cancellationToken);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(VendorMessages.VendorNotFound);
        }
    }
}