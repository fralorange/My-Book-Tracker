using MyELib.Application.AppData.Contexts.Document.Repositories;
using System.Linq.Expressions;
using DocumentEntity = MyELib.Domain.Document.Document;

namespace MyELib.Infrastructure.DataAccess.Contexts.Document.Repositories
{
    /// <inheritdoc cref="IDocumentRepository"/>
    [Obsolete("Migrated to EFCore. Use DocumentRepository Instead!")]
    public class DocumentInMemoryRepository : IDocumentRepository
    {
        private readonly List<DocumentEntity> _documents = [];

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<DocumentEntity>> GetAllAsync(CancellationToken token)
        {
            return Task.Run(() => (IReadOnlyCollection<DocumentEntity>) _documents, token);
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<DocumentEntity>> GetAllFilteredAsync(Expression<Func<DocumentEntity, bool>> expression, CancellationToken token)
        {
            return Task.Run(() => (IReadOnlyCollection<DocumentEntity>)_documents.Where(expression.Compile()).ToArray(), token);
        }

        /// <inheritdoc/>
        public Task<DocumentEntity?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return Task.Run(() => _documents.FirstOrDefault(doc => doc.Id == id), token);
        }

        /// <inheritdoc/>
        public Task<Guid> CreateAsync(DocumentEntity entity, CancellationToken token)
        {
            entity.Id = Guid.NewGuid();
            return Task.Run(() =>
            {
                _documents.Add(entity);
                return entity.Id;
            }, token);
        }

        /// <inheritdoc/>
        public Task UpdateAsync(DocumentEntity entity, CancellationToken token)
        {
            return Task.Run(() =>
            {
                var index = _documents.FindIndex(doc => doc.Id == entity.Id);
                _documents[index] = entity;
            }, token);
        }

        /// <inheritdoc/>
        public Task DeleteAsync(DocumentEntity entity, CancellationToken token)
        {
            return Task.Run(() => _documents.RemoveAll(doc => doc.Id == entity.Id), token);
        }

        /// <inheritdoc/>
        public Task<bool> ExistsAsync(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
