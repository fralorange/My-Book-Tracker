using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MyELib.Application.AppData.Contexts.LibraryUser.Repositories;
using MyELib.Application.AppData.Contexts.LibraryUser.Services;
using MyELib.Contracts.LibraryUser;
using MyELib.Domain.Identity;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.LibraryUser;
using System.Security.Claims;
using LibraryUserEntity = MyELib.Domain.LibraryUser.LibraryUser;

namespace MyELib.Application.Tests.Contexts.LibraryUser
{
    public class LibraryUserServiceTests
    {
        [Fact]
        public async Task GetLibraryUserById_ReturnCorrectLibraryUser()
        {
            // Arrange
            var libraryUserMapper = new LibraryUserMapper();

            var id = Guid.NewGuid();
            var expected = new LibraryUserEntity { Id = id };

            var httpContextMock = new Mock<IHttpContextAccessor>();
            var libraryUserRepositoryMock = new Mock<ILibraryUserRepository>();
            libraryUserRepositoryMock
                .Setup(lur => lur.GetByIdAsync(id, CancellationToken.None))
                .ReturnsAsync(expected);

            var service = new LibraryUserService(libraryUserRepositoryMock.Object, libraryUserMapper, httpContextMock.Object);

            // Act
            var actual = await service.GetByIdAsync(id, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(libraryUserMapper.MapToDto(expected));
        }

        [Fact]
        public async Task CreateNewLibraryUser_CreatedSuccessfully()
        {
            // Arrange
            var libraryUserMapper = new LibraryUserMapper();

            var libraryId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var role = Contracts.Identity.AuthRolesDto.Writer;
            var entityRole = (AuthRoles)Enum.Parse(typeof(AuthRoles), role.ToString());
            var newLibraryUser = new CreateLibraryUserDto { LibraryId = libraryId, UserId = userId, Role = role };

            var entity = libraryUserMapper.MapToLibraryUser(newLibraryUser);

            var httpContextMock = new Mock<IHttpContextAccessor>();
            var libraryUserRepositoryMock = new Mock<ILibraryUserRepository>();
            libraryUserRepositoryMock
                .Setup(lur => lur.CreateAsync(It.IsAny<LibraryUserEntity>(), CancellationToken.None));

            var service = new LibraryUserService(libraryUserRepositoryMock.Object, libraryUserMapper, httpContextMock.Object);

            // Act
            await service.CreateAsync(newLibraryUser, CancellationToken.None);

            // Assert
            libraryUserRepositoryMock
                .Verify(lur => lur.CreateAsync(It.Is<LibraryUserEntity>(lu => lu.UserId == entity.UserId && lu.LibraryId == entity.LibraryId && lu.Role == entityRole), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CreateNewLibraryUserThroughCurrentUser_CreatedSuccessfully()
        {
            // Arrange
            var libraryUserMapper = new LibraryUserMapper();

            var libraryId = Guid.NewGuid();
            var newLibraryUser = new CreateCurrentLibraryUserDto { LibraryId = libraryId };

            var entity = libraryUserMapper.MapToLibraryUser(newLibraryUser);
            entity.UserId = Guid.NewGuid();

            var claim = new Claim(type: ClaimTypes.NameIdentifier, value: entity.UserId.ToString());

            var httpContextMock = new Mock<IHttpContextAccessor>();
            httpContextMock
                .Setup(c => c.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                .Returns(claim);
            var libraryUserRepositoryMock = new Mock<ILibraryUserRepository>();
            libraryUserRepositoryMock
                .Setup(lur => lur.CreateAsync(It.IsAny<LibraryUserEntity>(), CancellationToken.None));

            var service = new LibraryUserService(libraryUserRepositoryMock.Object, libraryUserMapper, httpContextMock.Object);

            // Act
            await service.CreateAsync(newLibraryUser, CancellationToken.None);

            // Assert
            libraryUserRepositoryMock
                .Verify(lur => lur.CreateAsync(It.Is<LibraryUserEntity>(lu => lu.UserId == entity.UserId && lu.Role == AuthRoles.Owner && lu.LibraryId == entity.LibraryId), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task PatchLibraryUser_PatchedSuccessfully()
        {
            // Arrange
            var libraryUserMapper = new LibraryUserMapper();

            var libraryUserId = Guid.NewGuid();


            var newRole = Contracts.Identity.AuthRolesDto.Writer;
            var updatedLibraryUser = new UpdateLibraryUserDto { Role = newRole };
            var existingLibraryUser = new LibraryUserEntity { Id = libraryUserId, Role = AuthRoles.Reader };

            var entity = libraryUserMapper.MapToLibraryUser(updatedLibraryUser);
            entity.Id = libraryUserId;

            var httpContextMock = new Mock<IHttpContextAccessor>();
            var libraryUserRepositoryMock = new Mock<ILibraryUserRepository>();
            libraryUserRepositoryMock
                .Setup(lur => lur.UpdateAsync(It.IsAny<LibraryUserEntity>(), CancellationToken.None));
            libraryUserRepositoryMock
                .Setup(lur => lur.GetByIdAsync(libraryUserId, CancellationToken.None))
                .ReturnsAsync(existingLibraryUser);

            var service = new LibraryUserService(libraryUserRepositoryMock.Object, libraryUserMapper, httpContextMock.Object);

            // Act
            await service.PatchAsync(libraryUserId, updatedLibraryUser, CancellationToken.None);

            // Assert
            libraryUserRepositoryMock
                .Verify(lur => lur.UpdateAsync(It.Is<LibraryUserEntity>(lu => lu.Role == (AuthRoles)newRole && lu.Id == libraryUserId), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task LibraryUserExists_LibraryUserExistsTrue()
        {
            // Arrange
            var libraryUserMapper = new LibraryUserMapper();

            var libraryUser = new LibraryUserEntity { Id = Guid.NewGuid(), Role = AuthRoles.Reader };

            var targetId = libraryUser.Id;

            var repositoryMock = new Mock<ILibraryUserRepository>();
            repositoryMock.Setup(r => r.ExistsAsync(targetId, CancellationToken.None)).ReturnsAsync(true);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();

            var service = new LibraryUserService(repositoryMock.Object, libraryUserMapper, contextAccessorMock.Object);

            // Act
            var result = await service.ExistsAsync(targetId, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteLibraryUser_LibraryUserDeletedSuccessfully()
        {
            // Arrange
            var libraryUserMapper = new LibraryUserMapper();

            var libraryUser1 = new LibraryUserEntity { Id = Guid.NewGuid(), Role = AuthRoles.Reader };

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var repositoryMock = new Mock<ILibraryUserRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(libraryUser1.Id, CancellationToken.None)).ReturnsAsync(libraryUser1);
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<LibraryUserEntity>(), CancellationToken.None));

            var service = new LibraryUserService(repositoryMock.Object, libraryUserMapper, contextAccessorMock.Object);

            // Act
            await service.DeleteAsync(libraryUser1.Id, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.DeleteAsync(It.Is<LibraryUserEntity>(doc => doc.Id == libraryUser1.Id), CancellationToken.None), Times.Once);
        }
    }
}
