using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyELib.Application.AppData.Contexts.User.Services;
using MyELib.Contracts.User;

namespace MyELib.Hosts.Api.Controllers
{
    /// <summary>
    /// Контроллер пользователей.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Инициализирует контроллер пользоваетелей.
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Возвращает текущего пользователя.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAsync(CancellationToken token)
        {
            var currentUser = await _userService.GetCurrentUser(token);
            return Ok(currentUser);
        }

        /// <summary>
        /// Редактирует авторизованного пользователя.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="model">Отредактированная модель.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(Guid id, [FromForm] UpdateUserDto model, CancellationToken token)
        {
            var currentUser = await _userService.GetCurrentUser(token);
            if (currentUser.Id != id)
                return Forbid();
            var userExists = await _userService.ExistsAsync(id, token);
            if (!userExists)
                return NotFound();

            await _userService.UpdateAsync(id, model, token);
            return NoContent();
        }

        /// <summary>
        /// Удаляет аккаунт авторизованного пользователя.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken token)
        {
            var currentUser = await _userService.GetCurrentUser(token);
            if (currentUser.Id != id)
                return Forbid();
            var userExists = await _userService.ExistsAsync(id, token);
            if (!userExists)
                return NotFound();

            await _userService.DeleteAsync(id, token);
            return NoContent();
        }
    }
}
