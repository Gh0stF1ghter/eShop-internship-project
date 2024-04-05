namespace Identity.Tests.UnitTests.ServicesTests
{
    public class TokenServiceTest
    {
        private readonly Mock<IConfiguration> _configurationMock = new();
        private readonly UserManagerMock _userManagerMock;

        public TokenServiceTest()
        {
            var store = new Mock<IUserStore<User>>();

            _userManagerMock = new(store.Object, null, null, null, null, null, null, null, null);

            _configurationMock.Setup(c => c["Jwt:Issuer"])
                .Returns("localhost");
            _configurationMock.Setup(c => c["Jwt:Audience"])
                .Returns("localhost");
            _configurationMock.Setup(c => c["Jwt:SecretKey"])
                .Returns("S0meS3cretesfeoopi#210932erf3fwr32e#r32ejiwoKey");

            _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(["admin"]);
        }

        [Theory]
        [InlineData("Username")]
        [InlineData(null)]
        public async Task CreateTokenAsync_ValidParameters_ReturnsTokenDto(string? name)
        {
            //Arrange
            User user = new()
            {
                Id = "qwerty",
                UserName = name,
                RefreshToken = null,
                RefreshTokenExpireTime = DateTime.UtcNow,
            };

            var tokenService = new TokenService(_configurationMock.Object, _userManagerMock.Object);

            //Act
            var response = await tokenService.CreateTokenAsync(user, true);

            //Assert
            response.Should()
                .BeOfType<TokenDTO>()
                .Which.RefreshToken.Should()
                .NotBeNull();
        }

        [Fact]
        public async Task RefreshTokenAsync_ValidParameters_ReturnsTokenDto()
        {
            //Arrange
            var tokenDto = new TokenDTO(
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI5OGJlNGUyMi0yNWI4LTRkZmItYjljOS01YWI3MzQ4YjA0OWEiLCJ1bmlxdWVfbmFtZSI6IlVzZXJuYW1lIiwicm9sZSI6ImFkbWluIiwibmJmIjoxNzEwNDQ1OTU4LCJleHAiOjE3MTA0NDY1NTgsImlhdCI6MTcxMDQ0NTk1OCwiaXNzIjoibG9jYWxob3N0IiwiYXVkIjoibG9jYWxob3N0In0.VmwVBiXBw8qxu06EO5rCQx_dPYp6qCca1HEDOVMzolA",
                "G98y2fY5mF9uYOANodY4yoWW11mwukve//onCIdvj2Q=");

            User user = new()
            {
                Id = "qwerty",
                UserName = "Username",
                RefreshToken = "G98y2fY5mF9uYOANodY4yoWW11mwukve//onCIdvj2Q=",
                RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7),
            };

            //Act
            _userManagerMock.FindByName(user);

            var tokenService = new TokenService(_configurationMock.Object, _userManagerMock.Object);

            var response = await tokenService.RefreshTokenAsync(tokenDto, token: default);

            //Assert
            response.Should()
                .BeOfType<TokenDTO>()
                .Which.RefreshToken.Should()
                .NotBeNull()
                .And
                .NotBeEquivalentTo(tokenDto.RefreshToken);

            response.AccessToken.Should()
                .NotBeNull()
                .And
                .NotBeEquivalentTo(tokenDto.AccessToken);
        }

        [Fact]
        public async Task RefreshTokenAsync_ExpiredRefreshToken_ThrowsRefreshTokenBadRequestException()
        {
            //Arrange
            var tokenDto = new TokenDTO(
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI5OGJlNGUyMi0yNWI4LTRkZmItYjljOS01YWI3MzQ4YjA0OWEiLCJ1bmlxdWVfbmFtZSI6IlVzZXJuYW1lIiwicm9sZSI6ImFkbWluIiwibmJmIjoxNzEwNDQ1OTU4LCJleHAiOjE3MTA0NDY1NTgsImlhdCI6MTcxMDQ0NTk1OCwiaXNzIjoibG9jYWxob3N0IiwiYXVkIjoibG9jYWxob3N0In0.VmwVBiXBw8qxu06EO5rCQx_dPYp6qCca1HEDOVMzolA",
                "G98y2fY5mF9uYOANodY4yoWW11mwukve//onCIdvj2Q=");

            User user = new()
            {
                Id = "qwerty",
                UserName = "Username",
                RefreshToken = "G98y2fY5mF9uYOANodY4yoWW11mwukve//onCIdvj2Q=",
                RefreshTokenExpireTime = DateTime.UtcNow,
            };

            _userManagerMock.FindByName(user);

            var tokenService = new TokenService(_configurationMock.Object, _userManagerMock.Object);

            //Act
            var response = async () => await tokenService.RefreshTokenAsync(tokenDto, token: default);

            //Assert
            await response.Should()
                .ThrowAsync<RefreshTokenBadRequestException>()
                .WithMessage(Messages.InvalidToken);
        }

        [Fact]
        public async Task RefreshTokenAsync_WrongRefreshToken_ThrowsRefreshTokenBadRequestException()
        {
            //Arrange
            var tokenDto = new TokenDTO(
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI5OGJlNGUyMi0yNWI4LTRkZmItYjljOS01YWI3MzQ4YjA0OWEiLCJ1bmlxdWVfbmFtZSI6IlVzZXJuYW1lIiwicm9sZSI6ImFkbWluIiwibmJmIjoxNzEwNDQ1OTU4LCJleHAiOjE3MTA0NDY1NTgsImlhdCI6MTcxMDQ0NTk1OCwiaXNzIjoibG9jYWxob3N0IiwiYXVkIjoibG9jYWxob3N0In0.VmwVBiXBw8qxu06EO5rCQx_dPYp6qCca1HEDOVMzolA",
                "G98y2fY5mF9uYOANodY4yoWW11mwukve//onCIdvj2Q=");

            User user = new()
            {
                Id = "qwerty",
                UserName = "Username",
                RefreshToken = "G98y2fY5mF9uYOANodY4yoWW11mwukve//onCIdvj2",
                RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7),
            };

            //Act
            _userManagerMock.FindByName(user);

            var tokenService = new TokenService(_configurationMock.Object, _userManagerMock.Object);

            var response = async () => await tokenService.RefreshTokenAsync(tokenDto, token: default);

            //Assert
            await response.Should()
                .ThrowAsync<RefreshTokenBadRequestException>()
                .WithMessage(Messages.InvalidToken);
        }
    }
}