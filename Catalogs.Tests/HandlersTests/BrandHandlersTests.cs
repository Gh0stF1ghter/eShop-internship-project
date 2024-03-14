using AutoMapper;
using Catalogs.Application.Comands.BrandCommands;
using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using Catalogs.Application.Handlers.BrandHandlers;
using Catalogs.Application.MappingProfiles;
using Catalogs.Application.Queries.BrandQueries;
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
    public class BrandHandlersTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly CancellationToken _cancellationToken = It.IsAny<CancellationToken>();

        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc =>
                mc.AddProfiles(
                    [
                        new BrandProfile(),
                        new ItemProfile()
                    ])));

        [Fact]
        public async Task CreateBrandHandler_ValidParameters_ReturnsBrandDto()
        {
            var brandCreateDto = new BrandManipulateDto("Foo");

            _unitOfWork.Setup(uof => uof.Brand.IsExistAsync(It.IsAny<Expression<Func<Brand, bool>>>(), _cancellationToken))
                .ReturnsAsync(false);

            var comand = new CreateBrandComand(brandCreateDto);
            var handler = new CreateBrandHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(comand, _cancellationToken);

            response.Should()
                .BeOfType<BrandDto>()
                .Which.Name.Should()
                .Be(brandCreateDto.Name);
        }

        [Fact]
        public async Task CreateBrandHandler_BrandExists_ThrowsBadRequestException()
        {
            var brandCreateDto = new BrandManipulateDto("Foo");

            _unitOfWork.Setup(uof => uof.Brand.IsExistAsync(It.IsAny<Expression<Func<Brand, bool>>>(), _cancellationToken))
                .ReturnsAsync(true);

            var comand = new CreateBrandComand(brandCreateDto);
            var handler = new CreateBrandHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(comand, _cancellationToken);

            await response.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage(BrandMessages.BrandExists);
        }

        [Fact]
        public async Task DeleteBrandHandler_ValidParameters_ReturnsNoContent()
        {
            var brandList = new List<Brand>
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

            _unitOfWork.Setup(uof => uof.Brand.GetBrandByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync(brandList[0]);

            _unitOfWork.Setup(uof => uof.Brand.Delete(brandList[0]))
                .Callback(() => brandList.Remove(brandList[0]));

            var comand = new DeleteBrandComand(1, false);
            var handler = new DeleteBrandHandler(_unitOfWork.Object);

            await handler.Handle(comand, _cancellationToken);

            brandList.Count.Should().Be(1);
        }

        [Fact]
        public async Task DeleteBrandHandler_InvalidId_ThrowsNotFoundException()
        {
            _unitOfWork.Setup(uof => uof.Brand.GetBrandByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync((Brand)null);

            var comand = new DeleteBrandComand(1, false);
            var handler = new DeleteBrandHandler(_unitOfWork.Object);

            var response = async () => await handler.Handle(comand, _cancellationToken);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BrandMessages.BrandNotFound);
        }

        [Fact]
        public async Task GetBrandHandler_ValidParameters_ReturnsBrandDto()
        {
            var brandList = new List<Brand>
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

            _unitOfWork.Setup(uof => uof.Brand.GetBrandByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync(brandList[0]);

            var query = new GetBrandQuery(1, false);
            var handler = new GetBrandHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(query, _cancellationToken);

            response.Should()
                .BeOfType<BrandDto>()
                .Which.Id.Should()
                .Be(1);
        }

        [Fact]
        public async Task GetBrandHandler_InvalidId_ReturnsBrandDto()
        {
            _unitOfWork.Setup(uof => uof.Brand.GetBrandByIdAsync(1, false, _cancellationToken))
                .ReturnsAsync((Brand)null);

            var query = new GetBrandQuery(1, false);
            var handler = new GetBrandHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(query, _cancellationToken);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BrandMessages.BrandNotFound);
        }

        [Fact]
        public async Task GetBrandsHandler_ValidParameters_ReturnsBrandDtoList()
        {
            var brandList = new List<Brand>
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

            _unitOfWork.Setup(uof => uof.Brand.GetAllBrandsAsync(false, _cancellationToken))
                .ReturnsAsync(brandList);

            var query = new GetBrandsQuery(false);
            var handler = new GetBrandsHandler(_unitOfWork.Object, _mapper);

            var response = await handler.Handle(query, _cancellationToken);

            response.Should()
                .BeOfType<List<BrandDto>>()
                .Which.Count.Should()
                .Be(2);
        }

        [Theory]
        [InlineData(0, "Loe")]
        [InlineData(1, "Doow")]
        public async Task UpdateBrandHandler_ValidParameters_ReturnsNoContent(int id, string newName)
        {
            var brandList = new List<Brand>
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

            var brandUpdateDto = new BrandManipulateDto(newName);

            _unitOfWork.Setup(uof => uof.Brand.GetBrandByIdAsync(id, true, _cancellationToken))
                .ReturnsAsync(brandList[id]);

            var comand = new UpdateBrandComand(id, brandUpdateDto, true);
            var handler = new UpdateBrandHandler(_unitOfWork.Object, _mapper);

            await handler.Handle(comand, _cancellationToken);

            brandList[id].Should()
                .BeOfType<Brand>()
                .Which.Name.Should()
                .Be(brandUpdateDto.Name);
        }

        [Fact]
        public async Task UpdateBrandHandler_InvalidId_ReturnsBrandDto()
        {

            var brandUpdateDto = new BrandManipulateDto("Foo");

            _unitOfWork.Setup(uof => uof.Brand.GetBrandByIdAsync(It.IsAny<int>(), true, _cancellationToken))
                .ReturnsAsync((Brand)null);

            var comand = new UpdateBrandComand(It.IsAny<int>(), brandUpdateDto, true);
            var handler = new UpdateBrandHandler(_unitOfWork.Object, _mapper);

            var response = async () => await handler.Handle(comand, _cancellationToken);

            await response.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(BrandMessages.BrandNotFound);
        }
    }
}
