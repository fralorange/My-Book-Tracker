using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MyELib.Application.AppData.Contexts.User.Repositories;
using MyELib.Application.AppData.Contexts.User.Services;
using MyELib.Contracts.User;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.User;
using System.Linq.Expressions;
using System.Security.Claims;
using UserEntity = MyELib.Domain.User.User;

namespace MyELib.Application.Tests.Contexts.User.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetAllUsers_ReturnAllUsers()
        {
            // Arrange
            var userMapper = new UserMapper();

            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "user1", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };
            var user2 = new UserEntity { Id = Guid.NewGuid(), Username = "user2", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None)).ReturnsAsync([user1, user2]);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();

            var service = new UserService(repositoryMock.Object, userMapper, contextAccessorMock.Object);

            var expected = (IReadOnlyCollection<UserDto>)[userMapper.MapToDto(user1), userMapper.MapToDto(user2)];

            // Act
            var actual = await service.GetAllAsync(CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAllFilteredUsers_ReturnAllFilteredUsers()
        {
            // Arrange
            var userMapper = new UserMapper();

            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "user1", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };
            var user2 = new UserEntity { Id = Guid.NewGuid(), Username = "user2", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };
            var user3 = new UserEntity { Id = Guid.NewGuid(), Username = "user3", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };
            var user4 = new UserEntity { Id = Guid.NewGuid(), Username = "user4", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };

            Expression<Func<UserDto, bool>> expression = u => u.Username.EndsWith('1') || u.Username.EndsWith('4');

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock
                .Setup(r => r.GetAllFilteredAsync(It.Is<Expression<Func<UserEntity, bool>>>(e => e.Compile()(user1) && e.Compile()(user4)), CancellationToken.None))
                .ReturnsAsync([user1, user4]);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();

            var service = new UserService(repositoryMock.Object, userMapper, contextAccessorMock.Object);

            var expected = (IReadOnlyCollection<UserDto>)
            [
                userMapper.MapToDto(user1),
                userMapper.MapToDto(user2),
                userMapper.MapToDto(user3),
                userMapper.MapToDto(user4)
            ];

            expected = expected.Where(expression.Compile()).ToArray();

            // Act
            var actual = await service.GetAllFilteredAsync(expression, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetUserById_ReturnCorrectUser()
        {
            // Arrange
            var userMapper = new UserMapper();

            var targetId = Guid.NewGuid();
            var user1 = new UserEntity { Id = targetId, Username = "user1", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(targetId, CancellationToken.None)).ReturnsAsync(user1);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();

            var service = new UserService(repositoryMock.Object, userMapper, contextAccessorMock.Object);

            var expected = userMapper.MapToDto(user1);

            // Act
            var actual = await service.GetByIdAsync(targetId, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetCurrentUser_ReturnCorrectUser()
        {
            // Arrange
            var userMapper = new UserMapper();

            var targetId = Guid.NewGuid();
            var user1 = new UserEntity { Id = targetId, Username = "user1", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(targetId, CancellationToken.None)).ReturnsAsync(user1);

            var claim = new Claim(type: ClaimTypes.NameIdentifier, value: targetId.ToString());

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            contextAccessorMock.Setup(c => c.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)).Returns(claim);

            var service = new UserService(repositoryMock.Object, userMapper, contextAccessorMock.Object);

            var expected = userMapper.MapToDto(user1);

            // Act
            var actual = await service.GetCurrentUser(CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateUser_UserUpdateSuccessfully()
        {
            // Arrange
            var userMapper = new UserMapper();

            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "user1", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };
            var updUser1 = new UpdateUserDto { Username = "user11", Email = "user11@gmail.com", Password = "qwerty12345678" };

            var targetId = user1.Id;

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<UserEntity>(), CancellationToken.None));
            repositoryMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(true);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();

            var service = new UserService(repositoryMock.Object, userMapper, contextAccessorMock.Object);

            // Act
            await service.UpdateAsync(targetId, updUser1, CancellationToken.None);

            // Assert
            repositoryMock
                .Verify(r => r.UpdateAsync(It.Is<UserEntity>(u => u.Id == targetId && u.Username == updUser1.Username && u.Email == updUser1.Email), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_UserDeletedSuccessfully()
        {
            // Arrange
            var userMapper = new UserMapper();

            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "user1", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };

            var targetId = user1.Id;

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(targetId, CancellationToken.None)).ReturnsAsync(user1);
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<UserEntity>(), CancellationToken.None));

            var contextAccessorMock = new Mock<IHttpContextAccessor>();

            var service = new UserService(repositoryMock.Object, userMapper, contextAccessorMock.Object);

            // Act
            await service.DeleteAsync(targetId, CancellationToken.None);

            // Assert
            repositoryMock
                .Verify(r => r.DeleteAsync(It.Is<UserEntity>(u => u.Id == user1.Id), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task UserExists_UserExistsTrue()
        {
            // Arrange
            var userMapper = new UserMapper();

            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "user1", HashedPassword = "gdssfgsd", Salt = "gdsgdsg", Email = "gdsgsdg@gmail.com" };

            var targetId = user1.Id;

            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(r => r.ExistsAsync(targetId, CancellationToken.None)).ReturnsAsync(true);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();

            var service = new UserService(repositoryMock.Object, userMapper, contextAccessorMock.Object);

            // Act
            var result = await service.ExistsAsync(targetId, CancellationToken.None);

            // Assert
            Assert.True(result);
        }
    }
}
