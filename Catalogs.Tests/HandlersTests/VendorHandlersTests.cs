using Catalogs.Application.Comands.VendorCommands;
using Catalogs.Application.Handlers.VendorHandlers;
using Catalogs.Application.Queries.VendorQueries;
using Catalogs.Domain.Entities.Constants.Messages;
using Catalogs.Tests.FakeDataGenerator;
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

        public VendorHandlersTests()
        {
            DataGenerator.InitBogusData();
        }

        [Fact]
        public async Task CreateVendorHandlerAsync_ValidParameters_ReturnsVendorDto()
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
        public async Task CreateVendorHandlerAsync_VendorExists_ThrowsBadRequestException()
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
        public async Task DeleteVendorHandlerAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var vendor = DataGenerator.Vendors[0];

            _unitOfWorkMock.GetVendorById(vendor);

            var comand = new DeleteVendorComand(1, false);
            var handler = new DeleteVendorHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteVendorHandlerAsync_InvalidId_ThrowsNotFoundException()
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
        public async Task GetVendorHandlerAsync_ValidParameters_ReturnsVendorDto()
        {
            //Arrange
            var vendor = DataGenerator.Vendors[0];

            _unitOfWorkMock.GetVendorById(vendor);

            var query = new GetVendorQuery(1, false);
            var handler = new GetVendorHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(query, _cancellationToken);

            //Assert
            response.Should()
                .BeOfType<VendorDto>()
                .Which.Id.Should()
                .Be(vendor.Id);
        }

        [Fact]
        public async Task GetVendorHandlerAsync_InvalidId_ThrowsNotFoundException()
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
        public async Task GetVendorsHandlerAsync_ValidParameters_ReturnsVendorDtoList()
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
        public async Task UpdateVendorHandlerAsync_ValidParameters_ReturnsNoContent(int id, string newName)
        {
            //Arrange
            var vendor = DataGenerator.Vendors[0];

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
        public async Task UpdateVendorHandlerAsync_InvalidId_ThrowsNotFoundException()
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