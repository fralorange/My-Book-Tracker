using Microsoft.AspNetCore.Mvc;
using MyELib.Application.AppData.Contexts.Library.Services;
using MyELib.Contracts.Library;

namespace MyELib.Hosts.Api.Controllers
{
    /// <summary>
    /// Контроллер библиотек.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        /// <summary>
        /// Инициализирует контроллер библиотек.
        /// </summary>
        /// <param name="libraryService">Сервис библиотек.</param>
        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        /// <summary>
        /// GET-метод контроллера библиотек для получения коллекции.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyCollection<LibraryDto>>> GetAsync(CancellationToken token)
        {
            var collection = await _libraryService.GetAllAsync(token);
            return Ok(collection);
        }

        /// <summary>
        /// GET-метод контроллера библиотек для получения модели по идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(LibraryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken token)
        {
            var model = await _libraryService.GetByIdAsync(id, token);
            return model is null ? NotFound() : Ok(model);
        }

        /// <summary>
        /// POST-метод контроллера библиотек для добавления новых библиотек.
        /// </summary>
        /// <param name="model">Добавляемая модель.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromForm] CreateLibraryDto model, CancellationToken token)
        {
            var modelId = await _libraryService.CreateAsync(model, token);
            return Created(nameof(PostAsync), modelId);
        }

        /// <summary>
        /// PUT-метод контроллера библиотек для редактирования библиотек.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="model">Отредактированная модель.</param>
        /// <param name="token">Токен отмены операциию</param>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(Guid id, [FromForm] UpdateLibraryDto model, CancellationToken token)
        {
            var library = await _libraryService.GetByIdAsync(id, token);
            if (library is null)
                return NotFound();
            await _libraryService.UpdateAsync(model, id, token);
            return NoContent();
        }

        /// <summary>
        /// DELETE-метод контроллера библиотек для удаления библиотек.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token)
        {
            var library = await _libraryService.GetByIdAsync(id, token);
            if (library is null)
                return NotFound();
            await _libraryService.DeleteAsync(id, token);
            return NoContent();
        }
    }
}
