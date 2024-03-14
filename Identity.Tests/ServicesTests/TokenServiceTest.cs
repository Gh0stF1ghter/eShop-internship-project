using FluentAssertions;
using Identity.BusinessLogic.DTOs;
using Identity.BusinessLogic.Services.Implementations;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Entities.Constants;
using Identity.DataAccess.Entities.Models;
using Identity.Tests.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Tests.ServicesTests
{
    public class TokenServiceTest
    {
        private readonly Mock<IConfiguration> _configurationMock = new();
        private readonly Mock<UserManager<User>> usermanagerMock = UserManagerMock.MockUserManager<User>([new()
            {
                Id = "qwerty",
                UserName = "Name",
                RefreshToken = null,
                RefreshTokenExpireTime = DateTime.UtcNow,
            }]);

        public TokenServiceTest()
        {
            _configurationMock.Setup(c => c["Jwt:Issuer"])
                .Returns("localhost");
            _configurationMock.Setup(c => c["Jwt:Audience"])
                .Returns("localhost");
            _configurationMock.Setup(c => c["Jwt:SecretKey"])
                .Returns("S0meS3cretesfeoopi#210932erf3fwr32e#r32ejiwoKey");

            usermanagerMock.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(["admin"]);

        }

        [Theory]
        [InlineData("Username")]
        [InlineData(null)]
        public async Task CreateTokenAsync_ValidParameters_ReturnsTokenDto(string? name)
        {
            User user = new()
            { 
                Id = "qwerty",
                UserName = name,
                RefreshToken = null,
                RefreshTokenExpireTime = DateTime.UtcNow,
            };

            List<string> roles =
            [
                "admin"
            ];

            var tokenService = new TokenService(_configurationMock.Object, usermanagerMock.Object);

            var response = await tokenService.CreateTokenAsync(user, true);

            response.Should()
                .BeOfType<TokenDTO>()
                .Which.RefreshToken.Should()
                .NotBeNull();
        }

        [Fact]
        public async Task RefreshTokenAsync_ValidParameters_ReturnsTokenDto()
        {
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

            usermanagerMock.Setup(um => um.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);

            var tokenService = new TokenService(_configurationMock.Object, usermanagerMock.Object);

            var response = await tokenService.RefreshTokenAsync(tokenDto, token: default);

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
    }
}
