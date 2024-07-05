using FluentAssertions;
using Moq;
using MyELib.Application.AppData.Contexts.Document.Repositories;
using MyELib.Application.AppData.Contexts.Document.Services;
using MyELib.Contracts.Document;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Document;
using System.Linq.Expressions;
using DocumentEntity = MyELib.Domain.Document.Document;

namespace MyELib.Application.Tests.Contexts.Document.Services
{
    public class DocumentServiceTests
    {
        [Fact]
        public async Task GetAllDocuments_ReturnAllDocuments()
        {
            // Arrange
            var documentMapper = new DocumentMapper();

            var doc1 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Книга 1",
                FileType = ".pdf",
                Content = [],
                Size = 10000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = new(),
            };

            var doc2 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Книга 2",
                FileType = ".pdf",
                Content = [],
                Size = 20000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = new(),
            };

            var repositoryMock = new Mock<IDocumentRepository>();
            repositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None)).ReturnsAsync([doc1, doc2]);

            var expected = (IReadOnlyCollection<DocumentDto>)[documentMapper.MapToDto(doc1), documentMapper.MapToDto(doc2)];

            var service = new DocumentService(repositoryMock.Object, documentMapper);

            // Act
            var actual = await service.GetAllAsync(CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAllFilteredDocument_ReturnFilteredDocuments()
        {
            // Arrange
            var documentMapper = new DocumentMapper();

            var doc1 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Книга 1",
                FileType = ".pdf",
                Content = [],
                Size = 10000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = new(),
            };

            var doc2 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Книга 2",
                FileType = ".pdf",
                Content = [],
                Size = 20000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = new(),
            };

            var doc3 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Книга 3",
                FileType = ".pdf",
                Content = [],
                Size = 20000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = new(),
            };

            var doc4 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Книга 4",
                FileType = ".pdf",
                Content = [],
                Size = 20000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = new(),
            };

            Expression<Func<DocumentDto, bool>> expression = d => d.Name.EndsWith('1') || d.Name.EndsWith('3');
            var mappedExpression = documentMapper.MapToExpression(expression);

            var repositoryMock = new Mock<IDocumentRepository>();
            repositoryMock
                .Setup(r => r.GetAllFilteredAsync(It.Is<Expression<Func<DocumentEntity, bool>>>(e => e.Compile()(doc1) && e.Compile()(doc3)), CancellationToken.None))
                .ReturnsAsync([doc1, doc3]);

            var expected = (IReadOnlyCollection<DocumentDto>)
            [
                documentMapper.MapToDto(doc1),
                documentMapper.MapToDto(doc2),
                documentMapper.MapToDto(doc3),
                documentMapper.MapToDto(doc4)
            ];

            expected = expected.Where(expression.Compile()).ToArray();

            var service = new DocumentService(repositoryMock.Object, documentMapper);

            // Act
            var actual = await service.GetAllFilteredAsync(expression, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetDocumentById_ReturnDocumentSuccessfully()
        {
            // Arrange
            var documentMapper = new DocumentMapper();

            var doc1 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Книга 1",
                FileType = ".pdf",
                Content = [],
                Size = 10000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = new(),
            };

            var doc2 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Книга 2",
                FileType = ".pdf",
                Content = [],
                Size = 20000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = new(),
            };

            var targetId = doc2.Id;

            var repositoryMock = new Mock<IDocumentRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(targetId, CancellationToken.None)).ReturnsAsync(doc2);

            var collection = (IReadOnlyCollection<DocumentDto>)
            [
                documentMapper.MapToDto(doc1), documentMapper.MapToDto(doc2)
            ];

            var expected = collection.FirstOrDefault(doc => doc.Id == targetId);

            var service = new DocumentService(repositoryMock.Object, documentMapper);

            // Act
            var actual = await service.GetByIdAsync(targetId, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateDocument_DocumentCreatedSuccessfully()
        {
            // Arrange
            var documentMapper = new DocumentMapper();

            var createDoc1 = new CreateDocumentDto { Name = "Документ 1" };
            var createDocMetadata1 = new CreateDocumentDtoMetadata { Content = [], FileType = ".pdf", Size = 10000, UploadedDate = DateTime.Now };

            var repositoryMock = new Mock<IDocumentRepository>();
            repositoryMock.Setup(r => r.CreateAsync(It.IsAny<DocumentEntity>(), CancellationToken.None));

            var service = new DocumentService(repositoryMock.Object, documentMapper);

            // Act
            await service.CreateAsync(createDoc1, createDocMetadata1, CancellationToken.None);

            // Assert
            repositoryMock
                .Verify(r => r.CreateAsync(It.Is<DocumentEntity>(doc => doc.Name == createDoc1.Name && doc.FileType == createDocMetadata1.FileType), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task PatchDocument_DocumentUpdatedSuccessfully()
        {
            // Arrange
            var documentMapper = new DocumentMapper();

            var existingDoc1 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Док 1",
                FileType = ".docx",
                Content = [],
                Size = 10000,
                UploadedDate = DateTime.Now,
                Library = new()
            };
            var updateLibrary = new UpdateDocumentDto { Name = "Новый документ 1" };

            var repositoryMock = new Mock<IDocumentRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(existingDoc1.Id, CancellationToken.None)).ReturnsAsync(existingDoc1);
            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<DocumentEntity>(), CancellationToken.None));

            var service = new DocumentService(repositoryMock.Object, documentMapper);

            // Act
            await service.PatchAsync(existingDoc1.Id, updateLibrary, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.UpdateAsync(It.Is<DocumentEntity>(doc => doc.Name == updateLibrary.Name), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteDocument_DocumentDeletedSuccessfully()
        {
            // Arrange
            var documentMapper = new DocumentMapper();

            var existingDoc1 = new DocumentEntity
            {
                Id = Guid.NewGuid(),
                Name = "Док 1",
                FileType = ".docx",
                Content = [],
                Size = 10000,
                UploadedDate = DateTime.Now,
                Library = new()
            };

            var repositoryMock = new Mock<IDocumentRepository>();
            repositoryMock.Setup(r => r.GetByIdAsync(existingDoc1.Id, CancellationToken.None)).ReturnsAsync(existingDoc1);
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<DocumentEntity>(), CancellationToken.None));

            var service = new DocumentService(repositoryMock.Object, documentMapper);

            // Act
            await service.DeleteAsync(existingDoc1.Id, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.DeleteAsync(It.Is<DocumentEntity>(doc => doc.Id == existingDoc1.Id), CancellationToken.None), Times.Once);
        }
    }
}
