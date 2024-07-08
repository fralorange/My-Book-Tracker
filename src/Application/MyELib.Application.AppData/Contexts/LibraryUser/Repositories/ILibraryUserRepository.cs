using LibraryUserEntity = MyELib.Domain.LibraryUser.LibraryUser;

namespace MyELib.Application.AppData.Contexts.LibraryUser.Repositories
{
    /// <summary>
    /// Репозиторий библиотек-пользователей.
    /// </summary>
    public interface ILibraryUserRepository
    {
        /// <summary>
        /// Возвращает сущность библиотеки-пользователя по уникальному идентфикатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns><see cref="LibraryUserEntity"/></returns>
        Task<LibraryUserEntity?> GetByIdAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Создаёт новую библиотеку-пользователя.
        /// </summary>
        /// <param name="entity">Создаваемая сущность.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns><see cref="Guid"/></returns>
        Task<Guid> CreateAsync(LibraryUserEntity entity, CancellationToken token);
        /// <summary>
        /// Редактирует существующую библиотеку-пользователя.
        /// </summary>
        /// <param name="entity">Отредактированная сущность.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task UpdateAsync(LibraryUserEntity entity, CancellationToken token);
        /// <summary>
        /// Удаляет библиотеку-пользователя.
        /// </summary>
        /// <param name="entity">Отредактированная сущность.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task DeleteAsync(LibraryUserEntity entity, CancellationToken token);
        /// <summary>
        /// Проверяет на сущность библиотеки-пользователя.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid id, CancellationToken token);
    }
}
