﻿using Catalogs.Application.Comands.VendorCommands;
using Catalogs.Application.Handlers.VendorHandlers;
using Catalogs.Application.Queries.VendorQueries;
using Catalogs.Tests.Mocks;

namespace Catalogs.Tests.HandlersTests
{
    public class VendorHandlersTests
    {
        private readonly UnitOfWorkMock _unitOfWorkMock = new();
        private readonly CancellationToken _cancellationToken = It.IsAny<CancellationToken>();

        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc =>
                mc.AddProfile(new VendorProfile())));

        [Fact]
        public async Task CreateVendorHandler_ValidParameters_ReturnsVendorDto()
        {
            //Arrange
            var vendorCreateDto = new VendorManipulateDto("Foo");

            _unitOfWorkMock.IsVendorExists(false);

            var comand = new CreateVendorComand(vendorCreateDto);
            var handler = new CreateVendorHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(comand, _cancellationToken);

            //Assert
            response.Should()
                .BeOfType<VendorDto>()
                .Which.Name.Should()
                .Be(vendorCreateDto.Name);
        }

        [Fact]
        public async Task CreateVendorHandler_VendorExists_ThrowsBadRequestException()
        {
            //Arrange
            var vendorCreateDto = new VendorManipulateDto("Foo");

            _unitOfWorkMock.IsVendorExists(true);

            var comand = new CreateVendorComand(vendorCreateDto);
            var handler = new CreateVendorHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(VendorMessages.VendorExists);
        }

        [Fact]
        public async Task DeleteVendorHandler_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var vendor = new Vendor
            {
                Id = 1,
                Name = "Foo",
            };

            _unitOfWorkMock.GetVendorById(vendor);

            var comand = new DeleteVendorComand(1, false);
            var handler = new DeleteVendorHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteVendorHandler_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetVendorById(null);

            var comand = new DeleteVendorComand(1, false);
            var handler = new DeleteVendorHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(VendorMessages.VendorNotFound);
        }

        [Fact]
        public async Task GetVendorHandler_ValidParameters_ReturnsVendorDto()
        {
            //Arrange
            var vendor = new Vendor
            {
                Id = 1,
                Name = "Foo"
            };

            _unitOfWorkMock.GetVendorById(vendor);

            var query = new GetVendorQuery(1, false);
            var handler = new GetVendorHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(query, _cancellationToken);

            //Assert
            response.Should()
                .BeOfType<VendorDto>()
                .Which.Id.Should()
                .Be(1);
        }

        [Fact]
        public async Task GetVendorHandler_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetVendorById(null);

            var query = new GetVendorQuery(1, false);
            var handler = new GetVendorHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(query, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(VendorMessages.VendorNotFound);
        }

        [Fact]
        public async Task GetVendorsHandler_ValidParameters_ReturnsVendorDtoList()
        {
            //Arrange
            _unitOfWorkMock.GetAllVendors();

            var query = new GetVendorsQuery(false);
            var handler = new GetVendorsHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(query, _cancellationToken);

            //Assert
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
            //Arrange
            var vendor = new Vendor
            {
                Id = 1,
                Name = "Foo"
            };

            var endorUpdateDto = new VendorManipulateDto(newName);

            _unitOfWorkMock.GetVendorById(vendor);

            var comand = new UpdateVendorComand(id, endorUpdateDto, true);
            var handler = new UpdateVendorHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateVendorHandler_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            var vendorUpdateDto = new VendorManipulateDto("Foo");

            _unitOfWorkMock.GetVendorById(null);

            var comand = new UpdateVendorComand(It.IsAny<int>(), vendorUpdateDto, true);
            var handler = new UpdateVendorHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(VendorMessages.VendorNotFound);
        }
    }
}