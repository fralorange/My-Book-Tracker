using FluentAssertions;
using MyELib.Application.AppData.Contexts.Document.Repositories;
using MyELib.Infrastructure.DataAccess.Contexts.Document.Repositories;
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
            IDocumentRepository documentRepository = new DocumentInMemoryRepository();

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

            await documentRepository.CreateAsync(doc1, CancellationToken.None);
            await documentRepository.CreateAsync(doc2, CancellationToken.None);

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
            IDocumentRepository documentRepository = new DocumentInMemoryRepository();

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

            await documentRepository.CreateAsync(doc1, CancellationToken.None);
            await documentRepository.CreateAsync(doc2, CancellationToken.None);
            await documentRepository.CreateAsync(doc3, CancellationToken.None);
            await documentRepository.CreateAsync(doc4, CancellationToken.None);

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
            IDocumentRepository documentRepository = new DocumentInMemoryRepository();

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

            await documentRepository.CreateAsync(expected, CancellationToken.None);

            // Act
            var actual = await documentRepository.GetByIdAsync(expected.Id, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateDocument_DocumentCreatedSuccessfully()
        {
            // Arrange
            IDocumentRepository documentRepository = new DocumentInMemoryRepository();

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

            // Act
            await documentRepository.CreateAsync(doc1, CancellationToken.None);

            // Assert
            Assert.Contains(doc1, await documentRepository.GetAllAsync(CancellationToken.None));
        }

        [Fact]
        public async Task UpdateDocument_DocumentUpdatedSuccessfully()
        {
            // Arrange
            IDocumentRepository documentRepository = new DocumentInMemoryRepository();

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
            await documentRepository.CreateAsync(doc1, CancellationToken.None);
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

            // Act
            await documentRepository.UpdateAsync(updatedDoc1, CancellationToken.None);

            // Assert
            (await documentRepository.GetAllAsync(CancellationToken.None))
                .Should().Contain(l => l.Id == updatedDoc1.Id && l.Name == updatedDoc1.Name);
        }

        [Fact]
        public async Task DeleteDocument_DocumentDeletedSuccessfully()
        {
            // Arrange
            IDocumentRepository documentRepository = new DocumentInMemoryRepository();

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
            await documentRepository.CreateAsync(doc1, CancellationToken.None);

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
            Assert.DoesNotContain(doc1, await documentRepository.GetAllAsync(CancellationToken.None));
        }
    }
}
