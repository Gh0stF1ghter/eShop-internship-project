using AutoMapper;
using FluentAssertions;
using Identity.BusinessLogic.DTOs;
using Identity.BusinessLogic.Mapping;
using Identity.BusinessLogic.Services.Implementations;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities.Models;
using Identity.Tests.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Tests.ServicesTests
{
    public class AuthenticationServiceTest
    {
        private readonly Mock<UserManager<User>> _userManagerMock = UserManagerMock.MockUserManager<User>([new()
            {
                Id = "qwerty",
                UserName = "Name",
                RefreshToken = null,
                RefreshTokenExpireTime = DateTime.UtcNow,
            }]);

        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<ILogger<AuthenticationService>> _logger = new();

        private readonly Mapper _mapper = new(
            new MapperConfiguration(mc =>
            mc.AddProfile(new MappingProfile())));

        [Fact]
        public async Task AuthenticateAsync_ValidParameters_ReturnsTokenDto()
        {
            var loginDto = new LoginDTO("1234@12.com", "123456789", false);

            User user = new()
            {
                Id = "qwerty",
                UserName = "Username",
                Email = loginDto.Email,
                RefreshToken = null,
                RefreshTokenExpireTime = DateTime.UtcNow,
            };


            _userManagerMock.Setup(um => um.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginDto.Password))
                .ReturnsAsync(true);

            _tokenServiceMock.Setup(ts=> ts.CreateTokenAsync(user, true))
                .ReturnsAsync(new TokenDTO("123", "123"));

            var service = new AuthenticationService(_userManagerMock.Object, _tokenServiceMock.Object, _mapper, _logger.Object);

            var response = await service.AuthenticateAsync(loginDto, cancellationToken: default);

            response.Should().BeOfType<TokenDTO>();
        }

        [Fact]
        public async Task RegisterUserAsync_ValidParameters_ReturnsTrue()
        {
            var registerDto = new RegisterDTO("name", "mail", "12345678", "12345678");

            _userManagerMock.Setup(um => um.FindByEmailAsync(registerDto.Email))
                .ReturnsAsync((User)null);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            var service = new AuthenticationService(_userManagerMock.Object, _tokenServiceMock.Object, _mapper, _logger.Object);

            var response = await service.RegisterUserAsync(registerDto, token: default);

            response.Should().Be(true);

        }
    }
}