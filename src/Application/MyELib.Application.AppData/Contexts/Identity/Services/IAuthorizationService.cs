using MyELib.Contracts.Identity;

namespace MyELib.Application.AppData.Contexts.Identity.Services
{
    /// <summary>
    /// Сервис авторизации.
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Проверка доступа пользователя к ресурсу.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task<bool> HasAccessAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Проверка доступа пользователя к ресурсу с конкретой ролью.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="role">Требуемая роль.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task<bool> HasAccessAsync(Guid id, AuthRolesDto role, CancellationToken token);
        /// <summary>
        /// Проверка на то, что ресурс не имеет связей с пользователями.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task<bool> IsUserlessAsync(Guid id, CancellationToken token);
    }
}
