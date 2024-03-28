using Baskets.BusinessLogic.DataTransferObjects;
using Baskets.BusinessLogic.DataTransferObjects.CreateDTOs;
using Baskets.DataAccess.Entities.Models;
using Baskets.Tests.IntegrationTests.ApiFactory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace Baskets.Tests.IntegrationTests.ControllersTests
{
    [Collection(nameof(TestsCollection))]
    public class BasketItemControllerTests(BasketApiApplicationFactory basketApiApplicationFactory)
    {
        private readonly HttpClient _httpClient = basketApiApplicationFactory.CreateClient();

        private const string routeBase = "api/users";
        private const string validBasketItemsRoute = $"{routeBase}/65faeac0eb349fe87cd2f279/basket/items";

        [Fact]
        public async Task GetBasketItems_ValidParameters_ReturnsOk()
        {
            //Act
            var response = await _httpClient.GetAsync(validBasketItemsRoute);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetBasketItems_InvalidBasketId_ReturnsNotFound()
        {
            //Arrange
            var route = $"{routeBase}/69faeac0eb349fe87cd2f27a/basket/items";

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetBasketItem_ValidParameters_ReturnsOk()
        {
            //Arrange
            var route = $"{validBasketItemsRoute}/65fb182586219380db88b9fc";

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("69faeac0eb349fe87cd2f27a", "65fb182586219380db88b9fc")]
        [InlineData("65faeac0eb349fe87cd2f27a", "65fb182586919380db88b9fc")]
        public async Task GetBasketItem_InvalidBasketOrItemId_ReturnsNotFound(string userBasketId, string basketItemId)
        {
            //Arrange
            var route = $"{routeBase}/{userBasketId}/basket/items/{basketItemId}";

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateBasketItem_ValidParameters_ReturnsCreated()
        {
            //Arrange
            var basketRoute = $"{routeBase}/65faeac0eb349fe87cd2f279/basket";
            var requestBasket = await _httpClient.GetFromJsonAsync<UserBasketDto>(basketRoute);

            var request = new CreateBasketItemDto("65faedae6e5985f2047051b1");

            //Act
            var response = await _httpClient.PostAsJsonAsync(validBasketItemsRoute, request);

            var responseResource = await response.Content.ReadFromJsonAsync<BasketItem>();

            //Assert
            var responseBasket = await _httpClient.GetFromJsonAsync<UserBasketDto>($"{routeBase}/65faeac0eb349fe87cd2f279/basket");

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            responseResource.Should().BeEquivalentTo(request);
            responseBasket.TotalPrice.Should().Be(requestBasket.TotalPrice + responseResource.SumPrice);
        }

        [Theory]
        [InlineData("69faeac0eb349fe87cd2f27a", "65fb182586219380db88b9fc")]
        [InlineData("65faeac0eb349fe87cd2f27a", "65fb182586919380db88b9fc")]
        public async Task CreateBasketItem_InvalidBasketOrItemId_ReturnsNotFound(string userBasketId, string itemId)
        {
            //Arrange
            var route = $"{routeBase}/{userBasketId}/basket/items";

            var request = new CreateBasketItemDto(itemId);

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateBasketItem_ExistingBasketItem_ReturnsConflict()
        {
            //Arrange
            var request = new CreateBasketItemDto("65faedae6e5985f2047051af");

            //Act
            var response = await _httpClient.PostAsJsonAsync(validBasketItemsRoute, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task UpdateBasketItem_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var route = $"{validBasketItemsRoute}/65fb182586219380db88b9fc";

            var requestBasketItem = await _httpClient.GetFromJsonAsync<BasketItemDto>(route);
            var requestUserBasket = await _httpClient.GetFromJsonAsync<UserBasketDto>($"{routeBase}/65faeac0eb349fe87cd2f279/basket");

            var userBasketPriceWithoutItem = requestUserBasket.TotalPrice - requestBasketItem.SumPrice;

            var request = QueryHelpers.AddQueryString(route, "quantity", "4");
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, request);

            //Act
            var response = await _httpClient.SendAsync(requestMessage);

            //Assert
            var responseBasketItem = await _httpClient.GetFromJsonAsync<BasketItemDto>(route);
            var responseUserBasket = await _httpClient.GetFromJsonAsync<UserBasketDto>($"{routeBase}/65faeac0eb349fe87cd2f279/basket");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            responseBasketItem.SumPrice.Should().NotBe(requestBasketItem.SumPrice);
            responseUserBasket.TotalPrice.Should().Be(userBasketPriceWithoutItem + responseBasketItem.SumPrice);
        }

        [Theory]
        [InlineData("69faeac0eb349fe87cd2f27a", "65fb182586219380db88b9fc")]
        [InlineData("65faeac0eb349fe87cd2f27a", "65fb182586919380db88b9fc")]
        public async Task UpdateBasketItem_InvalidBasketOrBasketItemId_ReturnsNotFound(string userBasketId, string basketItemId)
        {
            //Arrange
            var route = $"{routeBase}/{userBasketId}/basket/items/{basketItemId}";

            var request = QueryHelpers.AddQueryString(route, "quantity", "4");
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, request);

            //Act
            var response = await _httpClient.SendAsync(requestMessage);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteBasketItem_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var route = $"{validBasketItemsRoute}/65fb185d86219380db88b9fd";

            var requestBasket = await _httpClient.GetFromJsonAsync<UserBasketDto>($"{routeBase}/65faeac0eb349fe87cd2f279/basket");
            var resourceOnDelete = await _httpClient.GetFromJsonAsync<BasketItemDto>(route);

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            var responseUserBasket = await _httpClient.GetFromJsonAsync<UserBasketDto>($"{routeBase}/65faeac0eb349fe87cd2f279/basket");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            responseUserBasket.TotalPrice.Should().Be(requestBasket.TotalPrice - resourceOnDelete.SumPrice);
        }

        [Theory]
        [InlineData("69faeac0eb349fe87cd2f27a", "65fb182586219380db88b9fc")]
        [InlineData("65faeac0eb349fe87cd2f27a", "65fb182586919380db88b9fc")]
        public async Task DeleteBasketItem_InvalidBasketOrBasketItemID_ReturnsNotFound(string userBasketId, string basketItemId)
        {
            //Arrange
            var route = $"{routeBase}/{userBasketId}/basket/items/{basketItemId}";

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}