using Microsoft.Extensions.Configuration;
using Moq;
using MyELib.Application.AppData.Contexts.Identity.Services;
using MyELib.Application.AppData.Contexts.User.Repositories;
using MyELib.Contracts.User;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.User;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using UserEntity = MyELib.Domain.User.User;

namespace MyELib.Application.Tests.Contexts.Identity.Services
{
    public class AuthenticationServiceTests
    {
        [Fact]
        public async Task LoginExistingUser_ReturnJWT()
        {
            // Arrange
            var userMapper = new UserMapper();

            var loginDto = new LoginUserDto { Username = "qwerty123", Password = "qwerty123" };

            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(loginDto.Password, salt);

            var existingUser = new UserEntity { Username = "qwerty123", Email = "qwerty123@mail.ru", Id = Guid.NewGuid(), HashedPassword = hashedPassword, Salt = salt };

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(r => r.GetByPredicateAsync(It.IsAny<Expression<Func<UserEntity, bool>>>(), CancellationToken.None)).ReturnsAsync(existingUser);

            var configurationMock = new Mock<IConfiguration>();

            var key = "d36d48fb559b7eaa5944095358c99166";
            var issuer = "JWTMyELIbAuthServer";
            var audience = "JWTMyELibServiceClient";

            configurationMock.Setup(c => c["Jwt:Key"]).Returns(key);
            configurationMock.Setup(c => c["Jwt:Issuer"]).Returns(issuer);
            configurationMock.Setup(c => c["Jwt:Audience"]).Returns(audience);

            var jwtHandler = new JwtSecurityTokenHandler();

            var service = new AuthenticationService(repositoryMock.Object, userMapper, configurationMock.Object);

            // Act
            var result = await service.LoginAsync(loginDto, CancellationToken.None);

            // Assert
            Assert.True(jwtHandler.CanReadToken(result));
        }

        [Fact]
        public async Task LoginNotExistingUser_ReturnsEmptyString()
        {
            // Arrange
            var userMapper = new UserMapper();

            var loginDto = new LoginUserDto { Username = "qwerty123", Password = "qwerty123" };

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(r => r.GetByPredicateAsync(It.IsAny<Expression<Func<UserEntity, bool>>>(), CancellationToken.None)).Returns(Task.FromResult<UserEntity?>(null));

            var configurationMock = new Mock<IConfiguration>();

            var key = "d36d48fb559b7eaa5944095358c99166";
            var issuer = "JWTMyELIbAuthServer";
            var audience = "JWTMyELibServiceClient";

            configurationMock.Setup(c => c["Jwt:Key"]).Returns(key);
            configurationMock.Setup(c => c["Jwt:Issuer"]).Returns(issuer);
            configurationMock.Setup(c => c["Jwt:Audience"]).Returns(audience);

            var jwtHandler = new JwtSecurityTokenHandler();

            var service = new AuthenticationService(repositoryMock.Object, userMapper, configurationMock.Object);

            // Act
            var result = await service.LoginAsync(loginDto, CancellationToken.None);

            // Assert
            Assert.True(!jwtHandler.CanReadToken(result) && string.IsNullOrEmpty(result));
        }

        [Fact]
        public async Task RegisterNewUser_NewUserCreatedSuccessfully()
        {
            // Arrange
            var userMapper = new UserMapper();

            var createDto = new CreateUserDto { Username = "qwerty123", Email = "qwerty123@mail.ru", Password = "qwerty123" };
            var userId = Guid.NewGuid();

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(r => r.CreateAsync(It.IsAny<UserEntity>(), CancellationToken.None))
                .ReturnsAsync(userId);

            var configurationMock = new Mock<IConfiguration>();

            var service = new AuthenticationService(repositoryMock.Object, userMapper, configurationMock.Object);

            // Act
            var result = await service.RegisterAsync(createDto, CancellationToken.None);

            // Assert
            Assert.Equal(userId, result);
        }
    }
}
