using MyELib.Application.AppData.Contexts.LibraryUser.Repositories;
using MyELib.Infrastructure.Repository;
using LibraryUserEntity = MyELib.Domain.LibraryUser.LibraryUser;

namespace MyELib.Infrastructure.DataAccess.Contexts.LibraryUser.Repositories
{
    /// <inheritdoc cref="ILibraryUserRepository"/>
    public class LibraryUserRepository : ILibraryUserRepository
    {
        private readonly IRepository<LibraryUserEntity> _repository;

        /// <summary>
        /// Инициализирует репозиторий библиотек-пользователей.
        /// </summary>
        /// <param name="repository"></param>
        public LibraryUserRepository(IRepository<LibraryUserEntity> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public Task<LibraryUserEntity?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return _repository.GetByIdAsync(id, token);
        }

        /// <inheritdoc/>
        public async Task<Guid> CreateAsync(LibraryUserEntity entity, CancellationToken token)
        {
            await _repository.AddAsync(entity, token);
            return entity.Id;
        }

        /// <inheritdoc/>
        public Task UpdateAsync(LibraryUserEntity entity, CancellationToken token)
        {
            return _repository.UpdateAsync(entity, token);
        }

        /// <inheritdoc/>
        public Task DeleteAsync(LibraryUserEntity entity, CancellationToken token)
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
