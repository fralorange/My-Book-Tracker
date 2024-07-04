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
        public Task CreateAsync(LibraryEntity entity, CancellationToken token)
        {
            return Task.Run(() => _libraries.Add(entity), token);
        }

        public Task UpdateAsync(LibraryEntity entity, CancellationToken token)
        {
            return Task.Run(async () =>
            {
                var currentEntity = await GetByIdAsync(entity.Id, token);
                currentEntity!.Name = entity.Name;
                currentEntity!.Documents = entity.Documents;
            }, token);
        }

        public Task DeleteAsync(LibraryEntity entity, CancellationToken token)
        {
            return Task.Run(() => _libraries.Remove(entity));
        }
    }
}
