using Microsoft.AspNetCore.Mvc;
using MyELib.Application.AppData.Contexts.Document.Services;
using MyELib.Application.AppData.Contexts.Document.Validator;
using MyELib.Contracts.Document;

namespace MyELib.Hosts.Api.Controllers
{
    /// <summary>
    /// Контроллер документов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IDocumentValidator _documentValidator;

        /// <summary>
        /// Инициализирует контроллер документов.
        /// </summary>
        /// <param name="documentService">Сервис для работы с документами.</param>
        /// <param name="documentValidator">Валидатор документов.</param>
        public DocumentController(IDocumentService documentService, IDocumentValidator documentValidator)
        {
            _documentService = documentService;
            _documentValidator = documentValidator;
        }

        /// <summary>
        /// Получить все документы.
        /// </summary>
        /// <param name="token"></param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyCollection<DocumentDto>>> GetAsync(CancellationToken token)
        {
            var collection = await _documentService.GetAllAsync(token);
            return Ok(collection);
        }

        /// <summary>
        /// Получить документ по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken token)
        {
            var model = await _documentService.GetByIdAsync(id, token);
            return model is null ? NotFound() : Ok(model);
        }

        /// <summary>
        /// Добавить новый документ.
        /// </summary>
        /// <param name="model">Модель.</param>
        /// <param name="file">Добавляемый файл приложенный к документу.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromForm] CreateDocumentDto model, IFormFile file, CancellationToken token)
        {
            if (!_documentValidator.TryValidateFile(file, out var metadata))
                return BadRequest();
            var modelId = await _documentService.CreateAsync(model, metadata, token);
            return Created(nameof(PostAsync), modelId);
        }

        /// <summary>
        /// Частично обновляет существующий документ.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="model">Отредактированная модель.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchAsync(Guid id, [FromForm] UpdateDocumentDto model, CancellationToken token)
        {
            var doc = await _documentService.GetByIdAsync(id, token);
            if (doc is null)
                return NotFound();
            await _documentService.PatchAsync(id, model, token);
            return NoContent();
        }

        /// <summary>
        /// Удаляет существующий документ.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken token)
        {
            var doc = await _documentService.GetByIdAsync(id, token);
            if (doc is null)
                return NotFound();
            await _documentService.DeleteAsync(id, token);
            return NoContent();
        }
    }
}
