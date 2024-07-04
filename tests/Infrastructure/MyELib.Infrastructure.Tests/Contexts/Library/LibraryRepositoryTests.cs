using FluentAssertions;
using MyELib.Application.AppData;
using MyELib.Infrastructure.DataAccess.Contexts.Library.Repositories;
using System.Linq.Expressions;
using LibraryEntity = MyELib.Domain.Library.Library;

namespace MyELib.Infrastructure.Tests.Contexts.Library
{
    public class LibraryRepositoryTests
    {
        [Fact]
        public async Task GetLibraries_ReturnAllLibraries()
        {
            // Arrange
            ILibraryRepository libraryRepository = new LibraryInMemoryRepository();

            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            var library2 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 2", Documents = [] };

            var expected = (IReadOnlyCollection<LibraryEntity>)[library1, library2];

            await libraryRepository.CreateAsync(library1, CancellationToken.None);
            await libraryRepository.CreateAsync(library2, CancellationToken.None);

            // Act
            var actual = await libraryRepository.GetAllAsync(CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetFilteredLibraries_ReturnAllFilteredLibraries()
        {
            // Arrange
            ILibraryRepository libraryRepository = new LibraryInMemoryRepository();

            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            var library2 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 2", Documents = [] };
            var library3 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 3", Documents = [] };
            var library4 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 4", Documents = [] };

            Expression<Func<LibraryEntity, bool>> expression = entity => entity.Name.EndsWith('1') || entity.Name.EndsWith('3');

            await libraryRepository.CreateAsync(library1, CancellationToken.None);
            await libraryRepository.CreateAsync(library2, CancellationToken.None);
            await libraryRepository.CreateAsync(library3, CancellationToken.None);
            await libraryRepository.CreateAsync(library4, CancellationToken.None);

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
            ILibraryRepository libraryRepository = new LibraryInMemoryRepository();

            var expected = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };

            await libraryRepository.CreateAsync(expected, CancellationToken.None);

            // Act
            var actual = await libraryRepository.GetByIdAsync(expected.Id, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateLibrary_LibraryCreatedSuccessfully()
        {
            // Arrange
            ILibraryRepository libraryRepository = new LibraryInMemoryRepository();

            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };

            // Act
            await libraryRepository.CreateAsync(library1, CancellationToken.None);

            // Assert
            Assert.Contains(library1, await libraryRepository.GetAllAsync(CancellationToken.None));
        }

        [Fact]
        public async Task UpdateLibrary_LibraryUpdateSuccessfully()
        {
            // Arrange
            ILibraryRepository libraryRepository = new LibraryInMemoryRepository();

            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            await libraryRepository.CreateAsync(library1, CancellationToken.None);
            var updatedLibrary1 = new LibraryEntity { Id = library1.Id, Name = "Обновленная библиотека 1", Documents = [] };

            // Act
            await libraryRepository.UpdateAsync(updatedLibrary1, CancellationToken.None);

            // Assert
            (await libraryRepository.GetAllAsync(CancellationToken.None))
                .Should().Contain(l => l.Id == updatedLibrary1.Id && l.Name == updatedLibrary1.Name);
        }

        [Fact]
        public async Task DeleteLibrary_LibraryDeletedSuccessfully()
        {
            // Arrange
            ILibraryRepository libraryRepository = new LibraryInMemoryRepository();

            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            await libraryRepository.CreateAsync(library1, CancellationToken.None);

            // Act
            await libraryRepository.DeleteAsync(library1, CancellationToken.None);

            // Assert
            Assert.DoesNotContain(library1, await libraryRepository.GetAllAsync(CancellationToken.None));
        }
    }
}
