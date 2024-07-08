using MyELib.Contracts.LibraryUser;

namespace MyELib.Application.AppData.Contexts.LibraryUser.Services
{
    /// <summary>
    /// Сервис библиотек-пользователей.
    /// </summary>
    public interface ILibraryUserService
    {
        /// <summary>
        /// Возвращает ДТО библиотеки-пользователя по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns><see cref="LibraryUserDto"/></returns>
        Task<LibraryUserDto?> GetByIdAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Создаёт новую библиотеку-пользователя для текущего пользователя.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns><see cref="Guid"/></returns>
        Task<Guid> CreateAsync(CreateCurrentLibraryUserDto dto, CancellationToken token);
        /// <summary>
        /// Создаёт новую библиотеку-пользователя для конкретного пользователя и выдаёт ему роль.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns><see cref="Guid"/></returns>
        Task<Guid> CreateAsync(CreateLibraryUserDto dto, CancellationToken token);
        /// <summary>
        /// Частично обновляет библиотеку-пользователя по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="dto">ДТО.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task PatchAsync(Guid id, UpdateLibraryUserDto dto, CancellationToken token);
        /// <summary>
        /// Удаляет библиотеку-пользователя.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Проверяет на сущность библиотеки-пользователя.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid id, CancellationToken token);
    }
}
