using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyELib.Application.AppData.Contexts.LibraryUser.Services;
using MyELib.Contracts.Identity;
using MyELib.Contracts.LibraryUser;
using System.Security.Claims;
using IAuthorizationService = MyELib.Application.AppData.Contexts.Identity.Services.IAuthorizationService;

namespace MyELib.Hosts.Api.Controllers
{
    /// <summary>
    /// Контроллер библиотек-пользователей.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LibraryUserController : ControllerBase
    {
        private readonly ILibraryUserService _libraryUserService;
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Инициализирует контроллер библиотек-пользователей.
        /// </summary>
        public LibraryUserController(ILibraryUserService libraryUserService, IAuthorizationService authorizationService)
        {
            _libraryUserService = libraryUserService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Создаёт библиотеку-пользователя для текущего пользователя и выдаёт максимальную роль.
        /// </summary>
        [HttpPost("init")]
        [ProducesResponseType(typeof(CreateLibraryUserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ActionName(nameof(InitAsync))]
        public async Task<IActionResult> InitAsync([FromForm] CreateCurrentLibraryUserDto model, CancellationToken token)
        {
            var perms = await _authorizationService.IsUserlessAsync(model.LibraryId, token);
            if (!perms)
                return Forbid();

            var modelId = await _libraryUserService.CreateAsync(model, token);
            return Created(nameof(InitAsync), modelId);
        }

        /// <summary>
        /// Выдаёт права доступа к библиотеке пользователю.
        /// </summary>
        [HttpPost("grant")]
        [ProducesResponseType(typeof(CreateLibraryUserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ActionName(nameof(PostAsync))]
        public async Task<IActionResult> PostAsync([FromForm] CreateLibraryUserDto model, CancellationToken token)
        {
            var perms = await _authorizationService.HasAccessAsync(model.LibraryId, AuthRolesDto.Owner, token);
            if (!perms)
                return Forbid();

            var modelId = await _libraryUserService.CreateAsync(model, token);
            return Created(nameof(PostAsync), modelId);
        }

        /// <summary>
        /// Частично редактирует библиотеку-пользователя (меняет уровень доступа [роль]).
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchAsync([FromForm] UpdateLibraryUserDto model, Guid id, CancellationToken token)
        {
            var entityExists = await _libraryUserService.ExistsAsync(id, token);
            if (!entityExists)
                return NotFound();

            var perms = await _authorizationService.HasAccessAsync(id, AuthRolesDto.Owner, token);
            if (!perms)
                return Forbid();

            await _libraryUserService.PatchAsync(id, model, token);
            return NoContent();
        }

        /// <summary>
        /// Удаляет библиотеку-пользователя (забирает доступ у пользователя).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpDelete("revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken token)
        {
            var entity = await _libraryUserService.GetByIdAsync(id, token);
            if (entity is null)
                return NotFound();

            var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var perms = await _authorizationService.HasAccessAsync(id, AuthRolesDto.Owner, token);
            if (!perms && entity.UserId == userId)
                return Forbid();

            await _libraryUserService.DeleteAsync(id, token);
            return NoContent();
        }
    }
}
