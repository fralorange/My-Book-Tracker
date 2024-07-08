using FluentAssertions;
using Moq;
using MyELib.Domain.Identity;
using MyELib.Infrastructure.DataAccess.Contexts.LibraryUser.Repositories;
using MyELib.Infrastructure.Repository;
using LibraryUserEntity = MyELib.Domain.LibraryUser.LibraryUser;

namespace MyELib.Infrastructure.Tests.Contexts.LibraryUser.Repositories
{
    public class LibraryUserRepositoryTests
    {
        [Fact]
        public async Task GetLibraryUserById_ReturnsCorrectLibraryUser()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var expected = new LibraryUserEntity { Id = targetId, Role = AuthRoles.Reader };

            var repositoryMock = new Mock<IRepository<LibraryUserEntity>>();
            repositoryMock.Setup(r => r.GetByIdAsync(targetId, CancellationToken.None))
                .ReturnsAsync(expected);

            var repository = new LibraryUserRepository(repositoryMock.Object);

            // Act
            var actual = await repository.GetByIdAsync(targetId, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateLibraryUser_LibraryUserCreatedSuccessfully()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var newLibraryUser = new LibraryUserEntity { Id = targetId, Role = AuthRoles.Reader };

            var repositoryMock = new Mock<IRepository<LibraryUserEntity>>();
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<LibraryUserEntity>(), CancellationToken.None));

            var repository = new LibraryUserRepository(repositoryMock.Object);

            // Act
            await repository.CreateAsync(newLibraryUser, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.AddAsync(It.Is<LibraryUserEntity>(lu => lu.Id == targetId), CancellationToken.None));
        }

        [Fact]
        public async Task UpdateLibraryUser_LibraryUserUpdatedSuccessfully()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var existingLibraryUser = new LibraryUserEntity { Id = targetId, Role = AuthRoles.Writer };
            var newRole = AuthRoles.Reader;
            var newLibraryUser = new LibraryUserEntity { Id = targetId, Role = newRole };

            var repositoryMock = new Mock<IRepository<LibraryUserEntity>>();
            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<LibraryUserEntity>(), CancellationToken.None));

            var repository = new LibraryUserRepository(repositoryMock.Object);

            // Act
            await repository.UpdateAsync(newLibraryUser, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.UpdateAsync(It.Is<LibraryUserEntity>(lu => lu.Id == targetId), CancellationToken.None));
        }

        [Fact]
        public async Task DeleteLibraryUser_LibraryUserDeletedSuccessfully()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var existingLibraryUser = new LibraryUserEntity { Id = targetId, Role = AuthRoles.Writer };

            var repositoryMock = new Mock<IRepository<LibraryUserEntity>>();
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<LibraryUserEntity>(), CancellationToken.None));

            var repository = new LibraryUserRepository(repositoryMock.Object);

            // Act
            await repository.DeleteAsync(existingLibraryUser, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.DeleteAsync(It.Is<LibraryUserEntity>(lu => lu.Id == targetId), CancellationToken.None));
        }

        [Fact]
        public async Task LibraryUserExists_LibraryUserExistsTrue()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var libraryUser = new LibraryUserEntity { Id = targetId, Role = AuthRoles.Writer };

            var repositoryMock = new Mock<IRepository<LibraryUserEntity>>();
            repositoryMock.Setup(r => r.ExistsAsync(targetId, CancellationToken.None)).ReturnsAsync(true);

            var libraryUserRepository = new LibraryUserRepository(repositoryMock.Object);

            // Act
            var result = await libraryUserRepository.ExistsAsync(targetId, CancellationToken.None);

            // Assert
            Assert.True(result);
        }
    }
}
