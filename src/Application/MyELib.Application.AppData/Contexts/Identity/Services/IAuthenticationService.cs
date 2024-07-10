using MyELib.Contracts.User;

namespace MyELib.Application.AppData.Identity.Services
{
    /// <summary>
    /// Сервис аутентификации и регистрации пользователей.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Зарегистрировать пользователя и сохранить его в БД.
        /// </summary>
        /// <param name="dto">Модель регистрации.</param>
        /// <param name="token">Токен отменны операции.</param>
        /// <returns><see cref="Guid"/> зарегистрированного пользователя.</returns>
        Task<Guid> RegisterAsync(CreateUserDto dto, CancellationToken token);
        /// <summary>
        /// Логин пользователя в систему.
        /// </summary>
        /// <param name="dto">Модель логина.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>JWT.</returns>
        Task<string> LoginAsync(LoginUserDto dto, CancellationToken token);
    }
}
