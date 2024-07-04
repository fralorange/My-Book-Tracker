using FluentAssertions;
using Moq;
using MyELib.Application.AppData;
using MyELib.Contracts.Library;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Library;
using System.Linq.Expressions;
using LibraryEntity = MyELib.Domain.Library.Library;

namespace MyELib.Application.Tests.Contexts.Library.Services
{
    public class LibraryServiceTests
    {
        [Fact]
        public async Task GetLibraries_ReturnAllLibraries()
        {
            // Arrange
            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            var library2 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 2", Documents = [] };

            var libraryMapper = new LibraryMapper();

            var repositoryMock = new Mock<ILibraryRepository>();
            repositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None)).ReturnsAsync([library1, library2]);

            var expected = (IReadOnlyCollection<LibraryDto>)[libraryMapper.MapToDto(library1), libraryMapper.MapToDto(library2)];

            var service = new LibraryService(repositoryMock.Object, libraryMapper);

            // Act
            var actual = await service.GetAllAsync(CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLibrariesFiltered_ReturnFilteredLibraries()
        {
            // Arrange
            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            var library2 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 2", Documents = [] };
            var library3 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 3", Documents = [] };
            var library4 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 4", Documents = [] };

            var libraryMapper = new LibraryMapper();

            Expression<Func<LibraryDto, bool>>  expression = dto => dto.Name.EndsWith('1') || dto.Name.EndsWith('3');
            var entityExpression = libraryMapper.MapToExpression(expression);

            var repositoryMock = new Mock<ILibraryRepository>();
            repositoryMock.Setup(r => r.GetAllFilteredAsync(
                It.Is<Expression<Func<LibraryEntity, bool>>>(e => e.Compile()(library1) && e.Compile()(library3)), CancellationToken.None)).ReturnsAsync([library1, library3]);

            var expected = (IReadOnlyCollection<LibraryDto>)
            [
                libraryMapper.MapToDto(library1), libraryMapper.MapToDto(library2), libraryMapper.MapToDto(library3), libraryMapper.MapToDto(library4)
            ];

            expected = expected.Where(expression.Compile()).ToArray();

            var service = new LibraryService(repositoryMock.Object, libraryMapper);

            // Act
            var actual = await service.GetAllFilteredAsync(expression, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetLibraryById_ReturnCorrectLibrary()
        {
            // Arrange
            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            var library2 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 2", Documents = [] };
            var library3 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 3", Documents = [] };

            var libraryMapper = new LibraryMapper();

            var targetId = library2.Id;

            var repositoryMock = new Mock<ILibraryRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(targetId, CancellationToken.None)).ReturnsAsync(library2);

            var collection = (IReadOnlyCollection<LibraryDto>)
            [
                libraryMapper.MapToDto(library1), libraryMapper.MapToDto(library2), libraryMapper.MapToDto(library3)
            ];

            var expected = collection.FirstOrDefault(lib => lib.Id == targetId);

            var service = new LibraryService(repositoryMock.Object, libraryMapper);

            // Act
            var actual = await service.GetByIdAsync(targetId, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateLibrary_LibraryCreatedSuccessfully()
        {
            // Arrange
            var libraryMapper = new LibraryMapper();

            var createLibrary1 = new CreateLibraryDto { Name = "Библиотека 1", Documents = [] };

            var repositoryMock = new Mock<ILibraryRepository>();
            repositoryMock.Setup(r => r.CreateAsync(It.IsAny<LibraryEntity>(), CancellationToken.None));

            var service = new LibraryService(repositoryMock.Object, libraryMapper);

            // Act
            await service.CreateAsync(createLibrary1, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.CreateAsync(It.Is<LibraryEntity>(l => l.Name == createLibrary1.Name), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task UpdateLibrary_LibraryUpdatedSuccessfully()
        {
            // Arrange
            var libraryMapper = new LibraryMapper();

            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };
            var updateLibrary1 = new UpdateLibraryDto { Name = "Обновленная библиотека 1", Documents = [] };

            var repositoryMock = new Mock<ILibraryRepository>();
            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<LibraryEntity>(), CancellationToken.None));
            repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(library1);

            var service = new LibraryService(repositoryMock.Object, libraryMapper);

            // Act
            await service.UpdateAsync(updateLibrary1, library1.Id, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.UpdateAsync(It.Is<LibraryEntity>(l => l.Name == updateLibrary1.Name), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteLibrary_LibraryDeletedSuccessfully()
        {
            // Arrange
            var libraryMapper = new LibraryMapper();

            var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Библиотека 1", Documents = [] };

            var repositoryMock = new Mock<ILibraryRepository>();
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<LibraryEntity>(), CancellationToken.None));
            repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(library1);

            var service = new LibraryService(repositoryMock.Object, libraryMapper);

            // Act
            await service.DeleteAsync(library1.Id, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.DeleteAsync(It.Is<LibraryEntity>(l => l.Id == library1.Id), CancellationToken.None), Times.Once);
        }
    }
}
