using FluentAssertions;
using Moq;
using MyELib.Application.AppData;
using MyELib.Infrastructure.DataAccess.Contexts.Library.Repositories;
using MyELib.Infrastructure.Repository;
using System.Linq.Expressions;
using LibraryEntity = MyELib.Domain.Library.Library;

namespace MyELib.Infrastructure.Tests.Contexts.Library.Repositories
{
    public class LibraryRepositoryTests
    {
        [Fact]
        public async Task GetLibraries_ReturnAllLibraries()
        {
            // Arrange
            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            var library2 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 2", Documents = [] };

            var repositoryMock = new Mock<IRepository<LibraryEntity>>();
            repositoryMock.Setup(r => r.GetAll()).Returns(new[] { library1, library2 }.AsQueryable());

            ILibraryRepository libraryRepository = new LibraryRepository(repositoryMock.Object);

            var expected = (IReadOnlyCollection<LibraryEntity>)[library1, library2];

            // Act
            var actual = await libraryRepository.GetAllAsync(CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetFilteredLibraries_ReturnAllFilteredLibraries()
        {
            // Arrange
            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            var library2 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 2", Documents = [] };
            var library3 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 3", Documents = [] };
            var library4 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 4", Documents = [] };

            var repositoryMock = new Mock<IRepository<LibraryEntity>>();
            repositoryMock
                .Setup(r => r.GetAllFiltered(It.Is<Expression<Func<LibraryEntity, bool>>>(e => e.Compile()(library1) && e.Compile()(library3))))
                .Returns(new[] { library1, library3 }.AsQueryable());

            ILibraryRepository libraryRepository = new LibraryRepository(repositoryMock.Object);

            Expression<Func<LibraryEntity, bool>> expression = entity => entity.Name.EndsWith('1') || entity.Name.EndsWith('3');

            var expected = (IReadOnlyCollection<LibraryEntity>)[library1, library2, library3, library4];
            expected = expected.Where(expression.Compile()).ToArray();

            // Act
            var actual = await libraryRepository.GetAllFilteredAsync(expression, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLibraryById_ReturnCorrectLibrary()
        {
            // Arrange
            var expected = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };

            var repositoryMock = new Mock<IRepository<LibraryEntity>>();
            repositoryMock.Setup(r => r.GetByIdAsync(expected.Id, CancellationToken.None))
                .ReturnsAsync(expected);

            ILibraryRepository libraryRepository = new LibraryRepository(repositoryMock.Object);

            // Act
            var actual = await libraryRepository.GetByIdAsync(expected.Id, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateLibrary_LibraryCreatedSuccessfully()
        {
            // Arrange
            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };

            var repositoryMock = new Mock<IRepository<LibraryEntity>>();
            repositoryMock.Setup(r => r.GetAll())
                .Returns(new[] { library1 }.AsQueryable());

            ILibraryRepository libraryRepository = new LibraryRepository(repositoryMock.Object);

            // Act
            var actual = await libraryRepository.CreateAsync(library1, CancellationToken.None);

            // Assert
            Assert.Contains(library1, await libraryRepository.GetAllAsync(CancellationToken.None));
        }

        [Fact]
        public async Task UpdateLibrary_LibraryUpdateSuccessfully()
        {
            // Arrange
            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            var updatedLibrary1 = new LibraryEntity { Id = library1.Id, Name = "Обновленная библиотека 1", Documents = [] };

            var repositoryMock = new Mock<IRepository<LibraryEntity>>();

            ILibraryRepository libraryRepository = new LibraryRepository(repositoryMock.Object);

            // Act
            await libraryRepository.UpdateAsync(updatedLibrary1, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.UpdateAsync(updatedLibrary1, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteLibrary_LibraryDeletedSuccessfully()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<LibraryEntity>>();

            ILibraryRepository libraryRepository = new LibraryRepository(repositoryMock.Object);

            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };

            // Act
            await libraryRepository.DeleteAsync(new LibraryEntity { Id = library1.Id, Name = library1.Name, Documents = library1.Documents }, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.DeleteAsync(It.Is<LibraryEntity>(lib => lib.Id == lib.Id), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task LibraryExists_LibraryExistsTrue()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var library1 = new LibraryEntity { Id = targetId, Name = "Библиотека 1", Documents = [] };

            var repositoryMock = new Mock<IRepository<LibraryEntity>>();
            repositoryMock.Setup(r => r.ExistsAsync(targetId, CancellationToken.None)).ReturnsAsync(true);

            var libraryUserRepository = new LibraryRepository(repositoryMock.Object);

            // Act
            var result = await libraryUserRepository.ExistsAsync(targetId, CancellationToken.None);

            // Assert
            Assert.True(result);
        }
    }
}
