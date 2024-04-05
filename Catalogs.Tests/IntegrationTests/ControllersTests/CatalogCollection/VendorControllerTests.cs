using Catalogs.Tests.IntegrationTests.ApiFactory;
using System.Net;
using System.Net.Http.Json;

namespace Catalogs.Tests.IntegrationTests.ControllersTests.CatalogCollection
{
    [Collection(nameof(CatalogCollection))]
    public class VendorControllerTests(CatalogApiApplicationFactory catalogApiApplicationFactory)
    {
        private readonly HttpClient _httpClient = catalogApiApplicationFactory.CreateClient();

        private const string routeBase = "api/vendors";
        private const string validIdRoute = $"{routeBase}/1";

        [Fact]
        public async Task GetAllVendorsAsync_ValidParameters_ReturnsOk()
        {
            //Act
            var response = await _httpClient.GetAsync(routeBase);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetVendorByIdAsync_ValidParameters_ReturnsOk()
        {
            //Act
            var response = await _httpClient.GetAsync(validIdRoute);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetVendorByIdAsync_InvalidVendorId_ReturnsNotFound()
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
        public async Task AddVendorAsync_ValidParameters_ReturnsCreated()
        {
            //Arrange
            var vendorToCreate = new VendorManipulateDto("Joe");

            //Act
            var response = await _httpClient.PostAsJsonAsync(routeBase, vendorToCreate);

            var responseResource = await response.Content.ReadFromJsonAsync<VendorDto>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            responseResource.Should().BeEquivalentTo(vendorToCreate);
        }

        [Theory]
        [InlineData("J")]
        [InlineData("")]
        public async Task AddVendorAsync_InvalidVendorName_ReturnsBadRequest(string name)
        {
            //Arrange
            var vendorToCreate = new VendorManipulateDto(name);

            //Act
            var response = await _httpClient.PostAsJsonAsync(routeBase, vendorToCreate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddVendorAsync_ExistingVendor_ReturnsConflict()
        {
            //Arrange
            var vendorToCreate = new VendorManipulateDto("Netflix");

            //Act
            var response = await _httpClient.PostAsJsonAsync(routeBase, vendorToCreate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task UpdateVendorAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var vendorToUpdate = new VendorManipulateDto("Jane");

            //Act
            var response = await _httpClient.PutAsJsonAsync(validIdRoute, vendorToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("")]
        [InlineData("J")]
        public async Task UpdateVendorAsync_InvalidVendorName_ReturnsBadRequest(string vendorName)
        {
            //Arrange
            var route = $"{routeBase}/1";
            var vendorToUpdate = new VendorManipulateDto(vendorName);

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, vendorToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateVendorAsync_InvalidVendorId_ReturnsNotFound()
        {
            //Arrange
            var route = $"{routeBase}/12";
            var vendorToUpdate = new VendorManipulateDto("Jane");

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, vendorToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task DeleteVendorAsync_ValidParameters_ReturnsOk()
        {
            //Arrange
            var deleteRoute = $"{routeBase}/5";

            //Act
            var response = await _httpClient.DeleteAsync(deleteRoute);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteVendorAsync_InvalidVendorId_ReturnsNotFound()
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