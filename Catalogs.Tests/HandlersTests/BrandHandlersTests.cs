using Catalogs.Application.Comands.BrandCommands;
using Catalogs.Application.Handlers.BrandHandlers;
using Catalogs.Application.Queries.BrandQueries;
using Catalogs.Tests.FakeData;
using Catalogs.Tests.Mocks;

namespace Catalogs.Tests.HandlersTests
{
    public class BrandHandlersTests
    {
        private readonly UnitOfWorkMock _unitOfWorkMock = new();
        private readonly CancellationToken _cancellationToken = It.IsAny<CancellationToken>();

        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc =>
                mc.AddProfiles(
                    [
                        new BrandProfile(),
                        new ItemProfile()
                    ])));

        public BrandHandlersTests() =>
            DataGenerator.InitBogusData();

        [Fact]
        public async Task CreateBrandHandlerAsync_ValidParameters_ReturnsBrandDto()
        {
            //Arrange
            var brandCreateDto = new BrandManipulateDto("Foo");

            _unitOfWorkMock.IsBrandExists(false);

            var comand = new CreateBrandComand(brandCreateDto);
            var handler = new CreateBrandHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(comand, _cancellationToken);

            //Assert
            response.Should()
                .BeOfType<BrandDto>()
                .Which.Name.Should()
                .Be(brandCreateDto.Name);
        }

        [Fact]
        public async Task CreateBrandHandlerAsync_BrandExists_ThrowsBadRequestException()
        {
            //Arrange
            var brandCreateDto = new BrandManipulateDto("Foo");

            _unitOfWorkMock.IsBrandExists(true);

            var comand = new CreateBrandComand(brandCreateDto);
            var handler = new CreateBrandHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(BrandMessages.BrandExists);
        }

        [Fact]
        public async Task DeleteBrandHandlerAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var brand = DataGenerator.Brands[0];

            _unitOfWorkMock.GetBrandById(brand);

            var comand = new DeleteBrandComand(1, false);
            var handler = new DeleteBrandHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteBrandHandlerAsync_InvalidId_ThrowsNotFoundException()
        {
            //Arrange
            _unitOfWorkMock.GetBrandById(null);

            var comand = new DeleteBrandComand(1, false);
            var handler = new DeleteBrandHandler(_unitOfWorkMock.Object);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BrandMessages.BrandNotFound);
        }

        [Fact]
        public async Task GetBrandHandlerAsync_ValidParameters_ReturnsBrandDto()
        {
            //Arrange
            var brand = DataGenerator.Brands[0];

            _unitOfWorkMock.Setup(uof => uof.Brand.GetBrandByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync(brand);

            var query = new GetBrandQuery(1, false);
            var handler = new GetBrandHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(query, _cancellationToken);

            //Assert
            response.Should()
                .BeOfType<BrandDto>();
        }

        [Fact]
        public async Task GetBrandHandlerAsync_InvalidId_ReturnsBrandDto()
        {
            //Arrange
            _unitOfWorkMock.GetBrandById(null);

            var query = new GetBrandQuery(1, false);
            var handler = new GetBrandHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(query, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BrandMessages.BrandNotFound);
        }

        [Fact]
        public async Task GetBrandsHandlerAsync_ValidParameters_ReturnsBrandDtoList()
        {
            //Arrange
            var brands = DataGenerator.Brands;

            _unitOfWorkMock.GetAllBrands(brands);

            var query = new GetBrandsQuery(false);
            var handler = new GetBrandsHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = await handler.Handle(query, _cancellationToken);

            //Assert
            response.Should()
                .BeOfType<List<BrandDto>>()
                .Which.Count.Should()
                .Be(brands.Count);
        }

        [Fact]
        public async Task UpdateBrandHandlerAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var brand = DataGenerator.Brands[0];

            var brandUpdateDto = new BrandManipulateDto("Doo");

            _unitOfWorkMock.GetBrandById(brand);

            var comand = new UpdateBrandComand(0, brandUpdateDto, true);
            var handler = new UpdateBrandHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateBrandHandlerAsync_InvalidId_ReturnsBrandDto()
        {
            //Arrange
            var brandUpdateDto = new BrandManipulateDto("Foo");

            _unitOfWorkMock.GetBrandById(null);

            var comand = new UpdateBrandComand(It.IsAny<int>(), brandUpdateDto, true);
            var handler = new UpdateBrandHandler(_unitOfWorkMock.Object, _mapper);

            //Act
            var response = async () => await handler.Handle(comand, _cancellationToken);

            //Assert
            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BrandMessages.BrandNotFound);
        }
    }
}