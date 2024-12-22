using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using MyELib.Application.AppData.Contexts.Document.Services;
using MyELib.Contracts.Document;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Document;
using MyELib.Infrastructure.DataAccess;
using MyELib.Infrastructure.DataAccess.Contexts.Document.Repositories;
using MyELib.Infrastructure.Repository;
using System.Security.Claims;
using DocumentEnt = MyELib.Domain.Document.Document;

namespace MyELib.Integration.Tests.Contexts.Document.Service
{
    public class DocumentServiceIntegrationTests
    {
        private DbContextOptions<BaseDbContext> CreateInMemoryDbOptions()
        {
            return new DbContextOptionsBuilder<BaseDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllDocuments_ShouldReturnAllDocuments()
        {
            var options = CreateInMemoryDbOptions();

            await using var dbContext = new BaseDbContext(options);
            var repository = new Repository<DocumentEnt>(dbContext);
            var documentRepository = new DocumentRepository(repository);
            var documentService = new DocumentService(documentRepository, new DocumentMapper(), null);

            var doc1 = new DocumentEnt { Id = Guid.NewGuid(), Name = "Document 1", FileType = ".pdf", Size = 10000, Content = [], UploadedBy = "12.02.2024" };
            var doc2 = new DocumentEnt { Id = Guid.NewGuid(), Name = "Document 2", FileType = ".docx", Size = 20000, Content = [], UploadedBy = "12.02.2024" };

            await dbContext.Set<DocumentEnt>().AddRangeAsync(doc1, doc2);
            await dbContext.SaveChangesAsync();

            var result = await documentService.GetAllAsync(CancellationToken.None);

            result.Should().HaveCount(2);
            result.Should().ContainEquivalentOf(new { Name = "Document 1" });
            result.Should().ContainEquivalentOf(new { Name = "Document 2" });
        }

        [Fact]
        public async Task GetDocumentById_ShouldReturnCorrectDocument()
        {
            var options = CreateInMemoryDbOptions();

            await using var dbContext = new BaseDbContext(options);
            var repository = new Repository<DocumentEnt>(dbContext);
            var documentRepository = new DocumentRepository(repository);
            var documentService = new DocumentService(documentRepository, new DocumentMapper(), null);

            var targetId = Guid.NewGuid();
            var doc = new DocumentEnt { Id = targetId, Name = "Target Document", FileType = ".txt", Size = 5000, Content = [], UploadedBy = "12.02.2024" };

            await dbContext.Set<DocumentEnt>().AddAsync(doc);
            await dbContext.SaveChangesAsync();

            var result = await documentService.GetByIdAsync(targetId, CancellationToken.None);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Target Document");
        }

        [Fact]
        public async Task CreateDocument_ShouldAddDocumentToDatabase()
        {
            var options = CreateInMemoryDbOptions();

            await using var dbContext = new BaseDbContext(options);
            var repository = new Repository<DocumentEnt>(dbContext);
            var documentRepository = new DocumentRepository(repository);
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "TestUser") };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthType"));
            var context = new DefaultHttpContext { User = user };
            contextAccessorMock.Setup(c => c.HttpContext).Returns(context);

            var documentService = new DocumentService(documentRepository, new DocumentMapper(), contextAccessorMock.Object);

            var createDto = new CreateDocumentDto { Name = "New Document" };
            var metadata = new CreateDocumentDtoMetadata
            {
                FileType = ".pdf",
                Size = 12000,
                Content = new byte[0],
                UploadedDate = DateTime.UtcNow
            };

            await documentService.CreateAsync(createDto, metadata, CancellationToken.None);

            var allDocuments = await dbContext.Set<DocumentEnt>().ToListAsync();
            allDocuments.Should().HaveCount(1);
            allDocuments.First().Name.Should().Be("New Document");
            allDocuments.First().UploadedBy.Should().Be("TestUser");
        }

        [Fact]
        public async Task UpdateDocument_ShouldModifyExistingDocument()
        {
            var options = CreateInMemoryDbOptions();

            await using var dbContext = new BaseDbContext(options);
            var repository = new Repository<DocumentEnt>(dbContext);
            var documentRepository = new DocumentRepository(repository);
            var documentService = new DocumentService(documentRepository, new DocumentMapper(), null);

            var targetId = Guid.NewGuid();
            var doc = new DocumentEnt { Id = targetId, Name = "Old Document", FileType = ".txt", Size = 3000, Content = [], UploadedBy = "12.02.2024" };

            await dbContext.Set<DocumentEnt>().AddAsync(doc);
            await dbContext.SaveChangesAsync();

            var updateDto = new UpdateDocumentDto { Name = "Updated Document" };

            await documentService.PatchAsync(targetId, updateDto, CancellationToken.None);

            var updatedDoc = await dbContext.Set<DocumentEnt>().FindAsync(targetId);
            updatedDoc.Should().NotBeNull();
            updatedDoc!.Name.Should().Be("Updated Document");
        }
    }
}
