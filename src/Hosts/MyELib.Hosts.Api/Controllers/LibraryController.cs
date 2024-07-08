using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyELib.Application.AppData.Contexts.Library.Services;
using MyELib.Contracts.Identity;
using MyELib.Contracts.Library;
using System.Security.Claims;
using X.PagedList;
using IAuthorizationService = MyELib.Application.AppData.Contexts.Identity.Services.IAuthorizationService;

namespace MyELib.Hosts.Api.Controllers
{
    /// <summary>
    /// Контроллер библиотек.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Инициализирует контроллер библиотек.
        /// </summary>
        /// <param name="libraryService">Сервис библиотек.</param>
        /// <param name="authorizationService">Сервис авторизации.</param>
        public LibraryController(ILibraryService libraryService, IAuthorizationService authorizationService)
        {
            _libraryService = libraryService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Получить все библиотеки.
        /// </summary>
        /// <param name="pageSize">Размер страницы.</param>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken token)
        {
            var currentUsedId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var collection = await _libraryService.GetAllAsync(token);
            // GetAllFiltered не работает с MapToExpression, переделывать дизайн на AutoMapper или писать собственный маппер Expression нет времени...
            var permissionCollection = collection.Where(lib => lib.LibraryUsers.Any(lu => lu.UserId == currentUsedId));
            var pagedCollection = permissionCollection.ToPagedList(pageNumber, pageSize);

            return Ok(pagedCollection);
        }

        /// <summary>
        /// Получить библиотеку по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(LibraryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken token)
        {
            var perms = await _authorizationService.HasAccessAsync(id, token);
            if (!perms)
                return Forbid();

            var model = await _libraryService.GetByIdAsync(id, token);
            return model is null ? NotFound() : Ok(model);
        }

        /// <summary>
        /// Добавить новую библиотеку.
        /// </summary>
        /// <param name="model">Добавляемая модель.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ActionName(nameof(PostAsync))]
        public async Task<IActionResult> PostAsync([FromForm] CreateLibraryDto model, CancellationToken token)
        {
            var modelId = await _libraryService.CreateAsync(model, token);
            return Created(nameof(PostAsync), modelId);
        }

        /// <summary>
        /// Обновить существующую библиотеку.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="model">Отредактированная модель.</param>
        /// <param name="token">Токен отмены операциию</param>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(Guid id, [FromForm] UpdateLibraryDto model, CancellationToken token)
        {
            var exists = await _libraryService.ExistsAsync(id, token);
            if (!exists)
                return NotFound();

            var perms = await _authorizationService.HasAccessAsync(id, AuthRolesDto.Writer, token);
            if (!perms)
                return Forbid();

            await _libraryService.UpdateAsync(model, id, token);
            return NoContent();
        }

        /// <summary>
        /// Удалить существующую библиотеку.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken token)
        {
            var exists = await _libraryService.ExistsAsync(id, token);
            if (!exists)
                return NotFound();

            var perms = await _authorizationService.HasAccessAsync(id, AuthRolesDto.Owner, token);
            if (!perms)
                return Forbid();

            await _libraryService.DeleteAsync(id, token);
            return NoContent();
        }
    }
}
