using FluentAssertions;
using Moq;
using MyELib.Application.AppData.Contexts.Document.Repositories;
using MyELib.Infrastructure.DataAccess.Contexts.Document.Repositories;
using MyELib.Infrastructure.Repository;
using System.Linq.Expressions;
using DocumentEntity = MyELib.Domain.Document.Document;

namespace MyELib.Infrastructure.Tests.Contexts.Document.Repositories
{
    public class DocumentRepositoryTests
    {
        [Fact]
        public async Task GetDocuments_ReturnAllDocuments()
        {
            // Arrange
            var doc1 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Док 1",
                FileType = ".docx",
                Content = [],
                Size = 10000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            var doc2 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Документ 2",
                FileType = ".pdf",
                Content = [],
                Size = 10000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            var repositoryMock = new Mock<IRepository<DocumentEntity>>();
            repositoryMock.Setup(r => r.GetAll()).Returns(new[] { doc1, doc2 }.AsQueryable());

            IDocumentRepository documentRepository = new DocumentRepository(repositoryMock.Object);

            var expected = (IReadOnlyCollection<DocumentEntity>)[doc1, doc2];

            // Act
            var actual = await documentRepository.GetAllAsync(CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetFilteredDocuments_ReturnAllFilteredDocuments()
        {
            // Arrange
            var doc1 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Док 1",
                FileType = ".docx",
                Content = [],
                Size = 10000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            var doc2 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Документ 2",
                FileType = ".pdf",
                Content = [],
                Size = 50000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            var doc3 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Док 3",
                FileType = ".docx",
                Content = [],
                Size = 10000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            var doc4 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Документ 4",
                FileType = ".doc",
                Content = [],
                Size = 20000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            Expression<Func<DocumentEntity, bool>> expression = doc => doc.FileType == ".docx";

            var repositoryMock = new Mock<IRepository<DocumentEntity>>();
            repositoryMock
                .Setup(r => r.GetAllFiltered(It.Is<Expression<Func<DocumentEntity, bool>>>(e => e.Compile()(doc1) && e.Compile()(doc3))))
                .Returns(new[] { doc1, doc3 }.AsQueryable());

            IDocumentRepository documentRepository = new DocumentRepository(repositoryMock.Object);

            var expected = (IReadOnlyCollection<DocumentEntity>)[doc1, doc2, doc3, doc4];
            expected = expected.Where(expression.Compile()).ToArray();

            // Act
            var actual = await documentRepository.GetAllFilteredAsync(expression, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetDocumentById_ReturnCorrectDocument()
        {
            // Arrange
            var expected = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Документ 4",
                FileType = ".doc",
                Content = [],
                Size = 20000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            var repositoryMock = new Mock<IRepository<DocumentEntity>>();
            repositoryMock.Setup(r => r.GetByIdAsync(expected.Id, CancellationToken.None))
                .ReturnsAsync(expected);

            IDocumentRepository documentRepository = new DocumentRepository(repositoryMock.Object);

            // Act
            var actual = await documentRepository.GetByIdAsync(expected.Id, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateDocument_DocumentCreatedSuccessfully()
        {
            // Arrange
            var doc1 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Документ 4",
                FileType = ".doc",
                Content = [111, 12, 87, 113, 10, 75],
                Size = 20000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            var repositoryMock = new Mock<IRepository<DocumentEntity>>();
            repositoryMock.Setup(r => r.GetAll())
                .Returns(new[] { doc1 }.AsQueryable());

            IDocumentRepository documentRepository = new DocumentRepository(repositoryMock.Object);

            // Act
            await documentRepository.CreateAsync(doc1, CancellationToken.None);

            // Assert
            Assert.Contains(doc1, await documentRepository.GetAllAsync(CancellationToken.None));
        }

        [Fact]
        public async Task UpdateDocument_DocumentUpdatedSuccessfully()
        {
            // Arrange
            var doc1 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Документ 4",
                FileType = ".doc",
                Content = [111, 12, 87, 113, 10, 75],
                Size = 20000,
                UploadedDate = DateTime.Now,
                Library = new()
            };
            var updatedDoc1 = new DocumentEntity
            {
                Id = doc1.Id,
                Name = "Обновленный документ 4",
                FileType = doc1.FileType,
                Content = doc1.Content,
                Size = doc1.Size,
                UploadedDate = doc1.UploadedDate,
                Library = doc1.Library,
            };

            var repositoryMock = new Mock<IRepository<DocumentEntity>>();

            IDocumentRepository documentRepository = new DocumentRepository(repositoryMock.Object);

            // Act
            await documentRepository.UpdateAsync(updatedDoc1, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.UpdateAsync(updatedDoc1, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteDocument_DocumentDeletedSuccessfully()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<DocumentEntity>>();

            IDocumentRepository documentRepository = new DocumentRepository(repositoryMock.Object);

            var doc1 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Документ 4",
                FileType = ".doc",
                Content = [111, 12, 87, 113, 10, 75],
                Size = 20000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            var targetDoc1 = new DocumentEntity
            {
                Id = doc1.Id,
                Name = doc1.Name,
                FileType = doc1.FileType,
                Content = doc1.Content,
                Size = doc1.Size,
                UploadedDate = doc1.UploadedDate,
                Library = doc1.Library,
            };

            // Act
            await documentRepository.DeleteAsync(targetDoc1, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.DeleteAsync(It.Is<DocumentEntity>(doc => doc.Id == doc.Id), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task LibraryExists_LibraryExistsTrue()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var doc1 = new DocumentEntity
            {
                Id = targetId,
                Name = "Документ 4",
                FileType = ".doc",
                Content = [111, 12, 87, 113, 10, 75],
                Size = 20000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            var repositoryMock = new Mock<IRepository<DocumentEntity>>();
            repositoryMock.Setup(r => r.ExistsAsync(targetId, CancellationToken.None)).ReturnsAsync(true);

            var libraryUserRepository = new DocumentRepository(repositoryMock.Object);

            // Act
            var result = await libraryUserRepository.ExistsAsync(targetId, CancellationToken.None);

            // Assert
            Assert.True(result);
        }
    }
}
