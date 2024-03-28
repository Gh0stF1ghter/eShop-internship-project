using Catalogs.Tests.IntegrationTests.ApiFactory;
using System.Net;
using System.Net.Http.Json;

namespace Catalogs.Tests.IntegrationTests.ControllersTests.CatalogCollection
{
    [Collection(nameof(CatalogCollection))]
    public class ItemTypeControllerTests(CatalogApiApplicationFactory catalogApiApplicationFactory)
    {
        private readonly HttpClient _httpClient = catalogApiApplicationFactory.CreateClient();

        private const string routeBase = "api/types";
        private const string validIdRoute = $"{routeBase}/1";

        [Fact]
        public async Task GetAllItemTypesAsync_ValidParameters_ReturnsOk()
        {
            //Act
            var response = await _httpClient.GetAsync(routeBase);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetItemTypeByIdAsync_ValidParameters_ReturnsOk()
        {
            //Act
            var response = await _httpClient.GetAsync(validIdRoute);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetItemTypeByIdAsync_InvalidItemTypeId_ReturnsNotFound()
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
        public async Task AddItemTypeAsync_ValidParameters_ReturnsCreated()
        {
            //Arrange
            var itemTypeToCreate = new ItemTypeManipulateDto("Joe");

            //Act
            var response = await _httpClient.PostAsJsonAsync(routeBase, itemTypeToCreate);

            var responseResource = await response.Content.ReadFromJsonAsync<ItemDto>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            responseResource.Should().BeEquivalentTo(itemTypeToCreate);
        }

        [Theory]
        [InlineData("J")]
        [InlineData("")]
        public async Task AddItemTypeAsync_InvalidItemTypeName_ReturnsBadRequest(string name)
        {
            //Arrange
            var itemTypeToCreate = new ItemTypeManipulateDto(name);

            //Act
            var response = await _httpClient.PostAsJsonAsync(routeBase, itemTypeToCreate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddItemTypeAsync_ExistingItemType_ReturnsConflict()
        {
            //Arrange
            var itemTypeToCreate = new ItemTypeManipulateDto("Electronics");

            //Act
            var response = await _httpClient.PostAsJsonAsync(routeBase, itemTypeToCreate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task UpdateItemTypeAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var itemTypeToUpdate = new ItemTypeManipulateDto("Jane");

            //Act
            var response = await _httpClient.PutAsJsonAsync(validIdRoute, itemTypeToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("", "1")]
        [InlineData("J", "1")]
        [InlineData("Jane", "12")]
        public async Task UpdateItemTypeAsync_InvalidItemTypeName_ReturnsBadRequest(string ItemTypeName, string ItemTypeId)
        {
            //Arrange
            var route = $"{routeBase}/{ItemTypeId}";
            var itemTypeToUpdate = new ItemTypeManipulateDto(ItemTypeName);

            //Act
            var response = await _httpClient.PutAsJsonAsync(route, itemTypeToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteItemTypeAsync_ValidParameters_ReturnsOk()
        {
            //Arrange
            var deleteRoute = $"{routeBase}/3";

            //Act
            var response = await _httpClient.DeleteAsync(deleteRoute);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteItemTypeAsync_InvalidItemTypeId_ReturnsNotFound()
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
