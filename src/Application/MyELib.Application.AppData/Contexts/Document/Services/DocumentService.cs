using Microsoft.AspNetCore.Http;
using MyELib.Application.AppData.Contexts.Document.Repositories;
using MyELib.Contracts.Document;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Document;
using System.Linq.Expressions;
using System.Security.Claims;

namespace MyELib.Application.AppData.Contexts.Document.Services
{
    /// <inheritdoc cref="IDocumentService"/>
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentMapper _documentMapper;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Инициализирует сервис документов.
        /// </summary>
        public DocumentService(IDocumentRepository documentRepository, IDocumentMapper documentMapper, IHttpContextAccessor contextAccessor)
        {
            _documentRepository = documentRepository;
            _documentMapper = documentMapper;
            _contextAccessor = contextAccessor;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<DocumentDto>> GetAllAsync(CancellationToken token)
        {
            var collection = await _documentRepository.GetAllAsync(token);
            return collection.Select(_documentMapper.MapToDto).ToArray();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<DocumentDto>> GetAllFilteredAsync(Expression<Func<DocumentDto, bool>> expression, CancellationToken token)
        {
            var mappedExpression = _documentMapper.MapToExpression(expression);
            var collection = await _documentRepository.GetAllFilteredAsync(mappedExpression, token);
            return collection.Select(_documentMapper.MapToDto).ToArray();
        }

        /// <inheritdoc/>
        public async Task<DocumentDto?> GetByIdAsync(Guid id, CancellationToken token)
        {
            var entity = await _documentRepository.GetByIdAsync(id, token);
            if (entity is null)
                return null;
            return _documentMapper.MapToDto(entity);
        }

        /// <inheritdoc/>
        public async Task<Guid> CreateAsync(CreateDocumentDto dto, CreateDocumentDtoMetadata metadata, CancellationToken token)
        {
            if (dto is null || metadata is null)
                return Guid.Empty;

            var username = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;

            var entity = _documentMapper.MapToDocument(dto, metadata);
            entity.UploadedBy = username;

            return await _documentRepository.CreateAsync(entity, token);
        }

        /// <inheritdoc/>
        public async Task PatchAsync(Guid id, UpdateDocumentDto dto, CancellationToken token)
        {
            var entity = await _documentRepository.GetByIdAsync(id, token);
            if (entity is null)
                return;

            entity.Name = dto.Name;

            await _documentRepository.UpdateAsync(entity, token);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var entity = await _documentRepository.GetByIdAsync(id, token);
            if (entity is null)
                return;
            await _documentRepository.DeleteAsync(entity, token);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Guid id, CancellationToken token)
        {
            return await _documentRepository.ExistsAsync(id, token);
        }
    }
}
