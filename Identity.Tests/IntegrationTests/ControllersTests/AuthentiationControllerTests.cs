using Identity.Tests.IntegrationTests.ApiFactory;
using System.Net;
using System.Net.Http.Json;

namespace Identity.Tests.IntegrationTests.ControllersTests
{
    public class AuthentiationControllerTests(IdentityApiApplicationFactory identityApiApplicationFactory) : IClassFixture<IdentityApiApplicationFactory>
    {
        private readonly HttpClient _httpClient = identityApiApplicationFactory.CreateClient();

        private const string routeBase = "api/authentication";

        [Fact]
        public async Task LoginAsync_ValidParameters_ReturnsOk()
        {
            //Arrange
            var route = $"{routeBase}/login";
            var request = new LoginDTO("user@example.com", "F_p@raD0x?", false);

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("uer@example.com", "F_p@raD0x?")]
        [InlineData("user@example.com", "F_p")]
        public async Task LoginAsync_InvalidParameters_ReturnsBadRequest(string email, string password)
        {
            //Arrange
            var route = $"{routeBase}/login";
            var request = new LoginDTO(email, password, false);

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterAsync_ValidParameters_ReturnsOk()
        {
            //Arrange
            var route = $"{routeBase}/register";
            var request = new RegisterDTO("JD0e", "JD0e@gmail.com", "F_p@raD0x?", "F_p@raD0x?");

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("", "JaneD0e@gmail.com", "F_p@raD0x?", "F_p@raD0x?")]
        [InlineData("JD0eJD0eJD0eJD0eJD0eJD0eJD0eJD0e", "JaneD0e@gmail.com", "F_p@raD0x?", "F_p@raD0x?")]
        [InlineData("JaneD0e", "", "F_p@raD0x?", "F_p@raD0x?")]
        [InlineData("JaneD0e", "JaneD0e@gmail.com", "F_", "F_")]
        [InlineData("JaneD0e", "JaneD0e@gmail.com", "F_paraD0x", "F_paraD0x")]
        [InlineData("JaneD0e", "JaneD0e@gmail.com", "F_paraD0x????????????", "F_paraD0x????????????")]
        [InlineData("JaneD0e", "JaneD0e@gmail.com", "F_p@raD0x?", "F_p@raD")]
        public async Task RegisterAsync_InvalidParameters_ReturnsBadRequest(string userName, string email, string password, string confirmPassword)
        {
            //Arrange
            var route = $"{routeBase}/register";
            var request = new RegisterDTO(userName, email, password, confirmPassword);

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterAsync_EmailTaken_ReturnsBadRequest()
        {
            //Arrange
            var route = $"{routeBase}/register";
            var request = new RegisterDTO("JaneD0e", "user@example.com", "F_p@raD0x?", "F_p@raD0x?");

            //Act
            var response = await _httpClient.PostAsJsonAsync(route, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

    }
}
