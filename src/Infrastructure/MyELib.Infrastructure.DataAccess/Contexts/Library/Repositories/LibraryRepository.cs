using MyELib.Application.AppData;
using MyELib.Infrastructure.Repository;
using System.Linq.Expressions;
using LibraryEntity = MyELib.Domain.Library.Library;

namespace MyELib.Infrastructure.DataAccess.Contexts.Library.Repositories
{
    /// <inheritdoc cref="ILibraryRepository"/>
    public class LibraryRepository : ILibraryRepository
    {
        private readonly IRepository<LibraryEntity> _repository;

        /// <summary>
        /// Инициализирует репозиторий библиотек.
        /// </summary>
        /// <param name="repository">Базовый репозиторий.</param>
        public LibraryRepository(IRepository<LibraryEntity> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<LibraryEntity>> GetAllAsync(CancellationToken token)
        {
            var collection = (IReadOnlyCollection<LibraryEntity>)_repository.GetAll().ToArray();
            return Task.Run(() => collection, token);
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<LibraryEntity>> GetAllFilteredAsync(Expression<Func<LibraryEntity, bool>> predicate, CancellationToken token)
        {
            var collection = (IReadOnlyCollection<LibraryEntity>)_repository.GetAll().Where(predicate).ToArray();
            return Task.Run(() => collection, token);
        }

        /// <inheritdoc/>
        public Task<LibraryEntity?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return _repository.GetByIdAsync(id, token);
        }

        /// <inheritdoc/>
        public Task<Guid> CreateAsync(LibraryEntity entity, CancellationToken token)
        {
            _repository.AddAsync(entity, token);
            return Task.FromResult(entity.Id);
        }

        /// <inheritdoc/>
        public Task UpdateAsync(LibraryEntity entity, CancellationToken token)
        {
            return _repository.UpdateAsync(entity, token);
        }

        /// <inheritdoc/>
        public Task DeleteAsync(LibraryEntity entity, CancellationToken token)
        {
            return _repository.DeleteAsync(entity, token);
        }
    }
}
