using AutoMapper;
using Identity.BusinessLogic.Mapping;
using Identity.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Identity.Tests.ServicesTests
{
    public class AuthenticationServiceTest
    {
        private readonly UserManagerMock _userManagerMock;

        public AuthenticationServiceTest()
        {
            var store = new Mock<IUserStore<User>>();

            _userManagerMock = new(store.Object, null, null, null, null, null, null, null, null);
        }

        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<ILogger<AuthenticationService>> _logger = new();

        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc =>
            mc.AddProfile(new MappingProfile())));

        [Fact]
        public async Task AuthenticateAsync_ValidParameters_ReturnsTokenDto()
        {
            //Arrange
            var loginDto = new LoginDTO("1234@12.com", "123456789", false);

            User user = new()
            {
                Id = "qwerty",
                UserName = "Username",
                Email = loginDto.Email,
                RefreshToken = null,
                RefreshTokenExpireTime = DateTime.UtcNow,
            };

            _userManagerMock.FindByEmail(user);
            _userManagerMock.CheckPassword(true);

            _tokenServiceMock.Setup(ts => ts.CreateTokenAsync(user, true))
                .ReturnsAsync(new TokenDTO("123", "123"));

            var service = new AuthenticationService(_userManagerMock.Object, _tokenServiceMock.Object, _mapper, _logger.Object);

            //Act
            var response = await service.AuthenticateAsync(loginDto, cancellationToken: default);

            //Assert
            response.Should().BeOfType<TokenDTO>();
        }

        [Fact]
        public async Task RegisterUserAsync_ValidParameters_ReturnsTrue()
        {
            //Arrange
            var registerDto = new RegisterDTO("name", "mail", "12345678", "12345678");

            _userManagerMock.FindByEmail(null);
            _userManagerMock.Create(IdentityResult.Success);

            var service = new AuthenticationService(_userManagerMock.Object, _tokenServiceMock.Object, _mapper, _logger.Object);

            //Act
            var response = await service.RegisterUserAsync(registerDto, token: default);

            //Assert
            response.Should().Be(true);
        }
    }
}