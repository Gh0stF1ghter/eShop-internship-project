using Catalogs.Tests.IntegrationTests.ApiFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Tests.IntegrationTests.ControllersTests
{
    public class ItemControllerTests(CatalogApiApplicationFactory catalogApiApplicationFactory) : IClassFixture<CatalogApiApplicationFactory>
    {
        private readonly HttpClient _httpClient = catalogApiApplicationFactory.CreateClient();

        private const string routeBase = "api/types";
        private const string validIdRoute = $"{routeBase}/1/items/2";

        [Fact]
        public async Task GetAllItemsAsync_ValidParameters_ReturnsOk()
        {
            //Arrange
            var route = $"{routeBase}/2";

            var request = new HttpRequestMessage(HttpMethod.Get, route);

            request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetItemByIdAsync_ValidParameters_ReturnsOk()
        {
            //Act
            var response = await _httpClient.GetAsync(validIdRoute);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetItemByIdAsync_InvalidItemId_ReturnsNotFound()
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
        public async Task AddItemAsync_ValidParameters_ReturnsCreated()
        {
            //Arrange
            var route = $"{routeBase}/2/items";
            var itemToCreate = new ItemManipulateDto("Jome", 1, 100, "img.jpg", 1, 1);

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, itemToCreate);

            var responseResource = await response.Content.ReadFromJsonAsync<ItemDto>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            responseResource.Should().BeEquivalentTo(itemToCreate);
        }

        [Theory]
        [InlineData("J", 1, 100)]
        [InlineData("", 1, 100)]
        [InlineData(".bgkrbfkldjgheurhperuhgidjhfgcvbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbiperh", 1, 100)]
        [InlineData("Photo", -1, 100)]
        [InlineData("Photo", 1, 0)]
        [InlineData("Photo", 1, -1)]
        public async Task AddItemAsync_InvalidItemParameters_ReturnsBadRequest(string name, int stock, double price)
        {
            //Arrange
            var route = $"{routeBase}/2/items";
            var itemToCreate = new ItemManipulateDto(name, stock, price, "img.jpg", 1, 1);

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, itemToCreate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(111, 1, 1)]
        [InlineData(1, 111, 1)]
        [InlineData(1, 1, 111)]
        public async Task AddItemAsync_InvalidIds_ReturnsNotFound(int typeId, int vendorId, int brandId)
        {
            //Arrange
            var route = $"{routeBase}/{typeId}/items";
            var itemToCreate = new ItemManipulateDto("Photo", 1, 100, "img.jpg", vendorId, brandId);

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, itemToCreate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddItemAsync_ExistingItem_ReturnsConflict()
        {
            //Arrange
            var route = $"{routeBase}/2/items";
            var itemToCreate = new ItemManipulateDto("Headphones", 15, 49.99, "headphones.jpg", 5, 2);

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, itemToCreate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task UpdateItemAsync_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var itemToUpdate = new ItemManipulateDto("Jane", 1, 100, "img.jpg", 1, 1);

            //Act
            var response = await _httpClient.PutAsJsonAsync(validIdRoute, itemToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("J", 1, 100, 1, 1)]
        [InlineData("", 1, 100, 1, 1)]
        [InlineData(".bgkrbfkldjgheurhperuhgidjhfgcvbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbiperh", 1, 100, 1, 1)]
        [InlineData("Photo", -1, 100, 1, 1)]
        [InlineData("Photo", 1, 0, 1, 1)]
        [InlineData("Photo", 1, -1, 1, 1)]
        [InlineData("Photo", 1, -1, 11, 1)]
        [InlineData("Photo", 1, -1, 1, 11)]
        public async Task UpdateItemAsync_InvalidItemParameters_ReturnsBadRequest(string name, int stock, double price, int vendorId, int brandId)
        {
            //Arrange
            var itemToUpdate = new ItemManipulateDto(name, stock, price, "img.jpg", vendorId, brandId);

            //Act
            var response = await _httpClient.PutAsJsonAsync(validIdRoute, itemToUpdate);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteItemAsync_ValidParameters_ReturnsOk()
        {
            //Arrange
            var deleteRoute = $"{routeBase}/3";

            //Act
            var response = await _httpClient.DeleteAsync(deleteRoute);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteItemAsync_InvalidItemId_ReturnsNotFound()
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
