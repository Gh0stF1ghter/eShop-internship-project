using Catalogs.Tests.IntegrationTests.ApiFactory;
using System.Net;
using System.Net.Http.Json;

namespace Catalogs.Tests.IntegrationTests.ControllersTests.CatalogCollection
{
    [Collection(nameof(CatalogCollection))]
    public class BrandControllerTests(CatalogApiApplicationFactory catalogApiApplicationFactory)
    {
        private readonly HttpClient _httpClient = catalogApiApplicationFactory.CreateClient();

        private const string routeBase = "api/brands";
        private const string validIdRoute = $"{routeBase}/1";

        [Fact]
        public async Task GetAllBrandsAsync_ValidParameters_ReturnsOk()
        {
            //Act
            var response = await _httpClient.GetAsync(routeBase);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetBrandByIdAsync_ValidParameters_ReturnsOk()
        {
            //Act
            var response = await _httpClient.GetAsync(validIdRoute);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetBrandByIdAsync_InvalidBrandId_ReturnsNotFound()
        {
            //Arrange
            var invalidIdRoute = $"{routeBase}/19";

            //Act
            var response = await _httpClient.GetAsync(invalidIdRoute);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddBrandAsync_ValidParameters_ReturnsCreated()
        {
            //Arrange
            var brandToCreate = new BrandManipulateDto("Joe");

            //Act
            var response = await _httpClient.PostAsJsonAsync(routeBase, brandToCreate);

            var responseResource = await response.Content.ReadFromJsonAsync<BrandDto>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            responseResource.Should().BeEquivalentTo(brandToCreate);
        }

        [Theory]
        [InlineData("J")]
        [InlineData("")]
        public async Task AddBrandAsync_InvalidBrandName_ReturnsBadRequest(string name)
        {
            //Arrange
            var brandToCreate = new BrandManipulateDto(name);

            //Act
            var response = await _httpClient.PostAsJsonAsync(routeBase, brandToCreate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddBrandAsync_ExistingBrand_ReturnsConflict()
        {
            //Arrange
            var brandToCreate = new BrandManipulateDto("CodeMaze");

            //Act
            var response = await _httpClient.PostAsJsonAsync(routeBase, brandToCreate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task UpdateBrandAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var brandToUpdate = new BrandManipulateDto("Jane");

            //Act
            var response = await _httpClient.PutAsJsonAsync(validIdRoute, brandToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("", "1")]
        [InlineData("J", "1")]
        [InlineData("Jane", "12")]
        public async Task UpdateBrandAsync_InvalidBrandName_ReturnsBadRequest(string brandName, string brandId)
        {
            //Arrange
            var route = $"{routeBase}/{brandId}";
            var brandToUpdate = new BrandManipulateDto(brandName);

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, brandToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteBrandAsync_ValidParameters_ReturnsOk()
        {
            //Arrange
            var deleteRoute = $"{routeBase}/5";

            //Act
            var response = await _httpClient.DeleteAsync(deleteRoute);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteBrandAsync_InvalidBrandId_ReturnsNotFound()
        {
            //Arrange
            var invalidDeleteRoute = $"{routeBase}/13";

            //Act
            var response = await _httpClient.DeleteAsync(invalidDeleteRoute);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}