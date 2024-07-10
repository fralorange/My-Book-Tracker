using MyELib.Application.AppData.Contexts.Document.Repositories;
using MyELib.Infrastructure.Repository;
using System.Linq.Expressions;
using DocumentEntity = MyELib.Domain.Document.Document;

namespace MyELib.Infrastructure.DataAccess.Contexts.Document.Repositories
{
    /// <inheritdoc cref="IDocumentRepository"/>
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IRepository<DocumentEntity> _repository;

        /// <summary>
        /// Инициализирует репозиторий документов.
        /// </summary>
        /// <param name="repository">Базовый репозиторий.</param>
        public DocumentRepository(IRepository<DocumentEntity> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<DocumentEntity>> GetAllAsync(CancellationToken token)
        {
            var collection = (IReadOnlyCollection<DocumentEntity>)_repository.GetAll().ToArray();
            return Task.Run(() => collection, token);
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<DocumentEntity>> GetAllFilteredAsync(Expression<Func<DocumentEntity, bool>> expression, CancellationToken token)
        {
            var collection = (IReadOnlyCollection<DocumentEntity>)_repository.GetAllFiltered(expression).ToArray();
            return Task.Run(() => collection, token);
        }

        /// <inheritdoc/>
        public Task<DocumentEntity?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return _repository.GetByIdAsync(id, token);
        }

        /// <inheritdoc/>
        public Task<Guid> CreateAsync(DocumentEntity entity, CancellationToken token)
        {
            _repository.AddAsync(entity, token);
            return Task.FromResult(entity.Id);
        }

        /// <inheritdoc/>
        public Task UpdateAsync(DocumentEntity entity, CancellationToken token)
        {
            return _repository.UpdateAsync(entity, token);
        }

        /// <inheritdoc/>
        public Task DeleteAsync(DocumentEntity entity, CancellationToken token)
        {
            return _repository.DeleteAsync(entity, token);
        }

        /// <inheritdoc/>
        public Task<bool> ExistsAsync(Guid id, CancellationToken token)
        {
            return _repository.ExistsAsync(id, token);
        }
    }
}
