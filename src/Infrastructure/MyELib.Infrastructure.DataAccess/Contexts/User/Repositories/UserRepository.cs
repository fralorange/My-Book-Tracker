using MyELib.Application.AppData.Contexts.User.Repositories;
using MyELib.Infrastructure.Repository;
using System.Linq.Expressions;
using UserEntity = MyELib.Domain.User.User;

namespace MyELib.Infrastructure.DataAccess.Contexts.User.Repositories
{
    /// <inheritdoc cref="IUserRepository"/>
    public class UserRepository : IUserRepository
    {
        private readonly IRepository<UserEntity> _repository;

        /// <summary>
        /// Инициализирует репозиторий пользователей.
        /// </summary>
        public UserRepository(IRepository<UserEntity> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<UserEntity>> GetAllAsync(CancellationToken token)
        {
            var collection = (IReadOnlyCollection<UserEntity>)_repository.GetAll().ToArray();
            return Task.Run(() => collection, token);
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<UserEntity>> GetAllFilteredAsync(Expression<Func<UserEntity, bool>> expression, CancellationToken token)
        {
            var collection = (IReadOnlyCollection<UserEntity>)_repository.GetAllFiltered(expression).ToArray();
            return Task.Run(() => collection, token);
        }

        /// <inheritdoc/>
        public Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return _repository.GetByIdAsync(id, token);
        }

        /// <inheritdoc/>
        public Task<UserEntity?> GetByPredicateAsync(Expression<Func<UserEntity, bool>> expression, CancellationToken token)
        {
            return _repository.GetByPredicateAsync(expression, token);
        }

        /// <inheritdoc/>
        public Task<Guid> CreateAsync(UserEntity user, CancellationToken token)
        {
            _repository.AddAsync(user, token);
            return Task.FromResult(user.Id);
        }

        /// <inheritdoc/>
        public Task UpdateAsync(UserEntity user, CancellationToken token)
        {
            return _repository.UpdateAsync(user, token);
        }

        /// <inheritdoc/>
        public Task DeleteAsync(UserEntity user, CancellationToken token)
        {
            return _repository.DeleteAsync(user, token);
        }

        /// <inheritdoc/>
        public Task<bool> ExistsAsync(Guid id, CancellationToken token)
        {
            return _repository.ExistsAsync(id, token);
        }
    }
}
