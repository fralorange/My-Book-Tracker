using MyELib.Contracts.User;
using System.Linq.Expressions;

namespace MyELib.Application.AppData.Contexts.User.Services
{
    /// <summary>
    /// Сервис для работы с пользователями.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Возвращает коллекцию пользователей.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Коллекцию <see cref="UserDto"/>.</returns>
        Task<IReadOnlyCollection<UserDto>> GetAllAsync(CancellationToken token);
        /// <summary>
        /// Возвращает отфильтрованную коллекцию
        /// </summary>
        /// <param name="expression">Параметр фильтрации.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Коллекцию <see cref="UserDto"/>.</returns>
        Task<IReadOnlyCollection<UserDto>> GetAllFilteredAsync(Expression<Func<UserDto, bool>> expression, CancellationToken token);
        /// <summary>
        /// Возвращает пользователя по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns><see cref="UserDto"/></returns>
        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Возвращает текущего пользователя.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns><see cref="UserDto"/></returns>
        Task<UserDto> GetCurrentUser(CancellationToken token);
        /// <summary>
        /// Редактирует пользователя.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="dto">Отредактированная модель.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task UpdateAsync(Guid id, UpdateUserDto dto, CancellationToken token);
        /// <summary>
        /// Удаляет пользователя.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task DeleteAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Проверяет на пользователя.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid id, CancellationToken token);
    }
}
