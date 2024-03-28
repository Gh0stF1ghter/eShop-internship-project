using Baskets.BusinessLogic.DataTransferObjects;
using Baskets.Tests.IntegrationTests.ApiFactory;
using System.Net.Http.Json;

namespace Baskets.Tests.IntegrationTests.ControllersTests
{
    [Collection(nameof(TestsCollection))]
    public class UserBasketContollerTests(BasketApiApplicationFactory basketApiApplicationFactory)
    {
        private readonly HttpClient _httpClient = basketApiApplicationFactory.CreateClient();

        private const string routeBase = "api/users";
        private const string validBasketRoute = $"{routeBase}/65faeac0eb349fe87cd2f279/basket";

        [Fact]
        public async Task GetUserBasket_ValidParameters_ReturnsOk()
        {
            //Act
            var response = await _httpClient.GetAsync(validBasketRoute);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetUserBasket_InvalidUserBasketId_ReturnsNotFound()
        {
            //Arrange
            var route = $"{routeBase}/65faeac5eb349fe87cd2f279/basket";

            //Act
            var response = await _httpClient.GetAsync(route);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateUserBasket_ValidParameters_ReturnsCreated()
        {
            //Arrange
            var route = $"{routeBase}/65faeac0eb349fe87cd2f27b/basket";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, route);

            //Act
            var response = await _httpClient.SendAsync(requestMessage);

            var responseResourse = response.Content.ReadFromJsonAsync<UserBasketDto>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            responseResourse.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateUserBasket_InvalidUserId_ReturnsNotFound()
        {
            //Arrange
            var route = $"{routeBase}/65faeac00b349fe87cd2f27b/basket";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, route);

            //Act
            var response = await _httpClient.SendAsync(requestMessage);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateUserBasket_BasketExists_ReturnsConflict()
        {
            //Arrange
            var route = $"{routeBase}/65faeac0eb349fe87cd2f279/basket";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, route);

            //Act
            var response = await _httpClient.SendAsync(requestMessage);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task DeleteUserBasket_ValidParameters_ReturnsNoContent()
        {
            //Arrange
            var route = $"{routeBase}/65faeac0eb349fe87cd2f27a/basket";

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteUserBasket_InvalidBasketId_ReturnsNotFound()
        {
            //Arrange
            var route = $"{routeBase}/65faeac0eb319fe87cd2f27a/basket";

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUserBasket_ExistingUserId_ReturnsBadRequest()
        {
            //Arrange
            var route = $"{routeBase}/65faeac0eb349fe87cd2f279/basket";

            //Act
            var response = await _httpClient.DeleteAsync(route);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}