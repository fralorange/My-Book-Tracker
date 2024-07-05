using MyELib.Application.AppData.Contexts.Document.Repositories;
using MyELib.Contracts.Document;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Document;
using System.Linq.Expressions;

namespace MyELib.Application.AppData.Contexts.Document.Services
{
    /// <inheritdoc cref="IDocumentService"/>
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentMapper _documentMapper;

        /// <summary>
        /// Инициализирует сервис документов.
        /// </summary>
        public DocumentService(IDocumentRepository documentRepository, IDocumentMapper documentMapper)
        {
            _documentRepository = documentRepository;
            _documentMapper = documentMapper;
        }

        public async Task<IReadOnlyCollection<DocumentDto>> GetAllAsync(CancellationToken token)
        {
            var collection = await _documentRepository.GetAllAsync(token);
            return collection.Select(_documentMapper.MapToDto).ToArray();
        }

        public async Task<IReadOnlyCollection<DocumentDto>> GetAllFilteredAsync(Expression<Func<DocumentDto, bool>> expression, CancellationToken token)
        {
            var mappedExpression = _documentMapper.MapToExpression(expression);
            var collection = await _documentRepository.GetAllFilteredAsync(mappedExpression, token);
            return collection.Select(_documentMapper.MapToDto).ToArray();
        }

        public async Task<DocumentDto?> GetByIdAsync(Guid id, CancellationToken token)
        {
            var entity = await _documentRepository.GetByIdAsync(id, token);
            if (entity is null)
                return null;
            return _documentMapper.MapToDto(entity);
        }

        public async Task<Guid> CreateAsync(CreateDocumentDto dto, CreateDocumentDtoMetadata metadata, CancellationToken token)
        {
            if (dto is null || metadata is null)
                return Guid.Empty;
            var entity = _documentMapper.MapToDocument(dto, metadata);
            return await _documentRepository.CreateAsync(entity, token);
        }
        public async Task PatchAsync(Guid id, UpdateDocumentDto dto, CancellationToken token)
        {
            var targetDto = await GetByIdAsync(id, token);
            if (targetDto is null)
                return;
            var entity = _documentMapper.MapToDocument(targetDto);

            entity.Name = dto.Name;

            await _documentRepository.UpdateAsync(entity, token);
        }

        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var targetDto = await GetByIdAsync(id, token);
            if (targetDto is null)
                return;
            var entity = _documentMapper.MapToDocument(targetDto);
            await _documentRepository.DeleteAsync(entity, token);
        }
    }
}
