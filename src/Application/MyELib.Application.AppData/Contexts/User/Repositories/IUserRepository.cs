using System.Linq.Expressions;
using UserEntity = MyELib.Domain.User.User;

namespace MyELib.Application.AppData.Contexts.User.Repositories
{
    /// <summary>
    /// Репозиторий для работы с  пользователями.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Возвращает коллекцию пользователей.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Коллекцию <see cref="UserEntity"/></returns>
        Task<IReadOnlyCollection<UserEntity>> GetAllAsync(CancellationToken token);
        /// <summary>
        /// Возвращает отфильтрованную коллекцию пользователей.
        /// </summary>
        /// <param name="expression">Параметр фильтрации.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Коллекцию <see cref="UserEntity"/></returns>
        Task<IReadOnlyCollection<UserEntity>> GetAllFilteredAsync(Expression<Func<UserEntity, bool>> expression, CancellationToken token);
        /// <summary>
        /// Возвращает пользователя по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns><see cref="UserEntity"/></returns>
        Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Вовзвращает пользователя по параметру (предикате).
        /// </summary>
        /// <param name="expression">Параметр поиска.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns><see cref="UserEntity"/></returns>
        Task<UserEntity?> GetByPredicateAsync(Expression<Func<UserEntity, bool>> expression, CancellationToken token);
        /// <summary>
        /// Добавляет нового пользователя.
        /// </summary>
        /// <param name="user">Добавляемый пользователь.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns><see cref="Guid"/> добавленного пользователя.</returns>
        Task<Guid> CreateAsync(UserEntity user, CancellationToken token);
        /// <summary>
        /// Редактирует существующего пользователя.
        /// </summary>
        /// <param name="user">Существующий пользователь.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task UpdateAsync(UserEntity user, CancellationToken token);
        /// <summary>
        /// Удаляет существующего пользователя.
        /// </summary>
        /// <param name="user">Существующий пользователь.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task DeleteAsync(UserEntity user, CancellationToken token);
        /// <summary>
        /// Проверяет на сущность пользователя.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid id, CancellationToken token);
    }
}
