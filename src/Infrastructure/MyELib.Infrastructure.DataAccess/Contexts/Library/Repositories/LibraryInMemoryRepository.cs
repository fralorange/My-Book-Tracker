using MyELib.Application.AppData;
using System.Linq.Expressions;
using LibraryEntity = MyELib.Domain.Library.Library;

namespace MyELib.Infrastructure.DataAccess.Contexts.Library.Repositories
{
    /// <inheritdoc cref="ILibraryRepository"/>
    public class LibraryInMemoryRepository : ILibraryRepository
    {
        private readonly List<LibraryEntity> _libraries = [];

        public Task<IReadOnlyCollection<LibraryEntity>> GetAllAsync(CancellationToken token)
        {
            return Task.Run(() => (IReadOnlyCollection<LibraryEntity>)_libraries.AsReadOnly(), token);
        }

        public Task<IReadOnlyCollection<LibraryEntity>> GetAllFilteredAsync(Expression<Func<LibraryEntity, bool>> predicate, CancellationToken token)
        {
            return Task.Run(() => (IReadOnlyCollection<LibraryEntity>)_libraries.Where(predicate.Compile()).ToArray(), token);
        }

        public Task<LibraryEntity?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return Task.Run(() => _libraries.FirstOrDefault(l => l.Id == id), token);
        }
        public Task<Guid> CreateAsync(LibraryEntity entity, CancellationToken token)
        {
            entity.Id = Guid.NewGuid();
            return Task.Run(() =>
            {
                _libraries.Add(entity);
                return entity.Id;
            }, token);
        }

        public Task UpdateAsync(LibraryEntity entity, CancellationToken token)
        {
            return Task.Run(() =>
            {
                var index = _libraries.FindIndex(lib => lib.Id == entity.Id);
                _libraries[index] = entity;
            }, token);
        }

        public Task DeleteAsync(LibraryEntity entity, CancellationToken token)
        {
            return Task.Run(() => _libraries.RemoveAll(lib => lib.Id == entity.Id));
        }
    }
}
