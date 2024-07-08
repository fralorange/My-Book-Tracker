using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyELib.Application.AppData.Contexts.Document.Services;
using MyELib.Application.AppData.Contexts.Document.Validator;
using MyELib.Contracts.Document;
using MyELib.Contracts.Identity;
using IAuthorizationService = MyELib.Application.AppData.Contexts.Identity.Services.IAuthorizationService;

namespace MyELib.Hosts.Api.Controllers
{
    /// <summary>
    /// Контроллер документов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IDocumentValidator _documentValidator;
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Инициализирует контроллер документов.
        /// </summary>
        /// <param name="documentService">Сервис для работы с документами.</param>
        /// <param name="documentValidator">Валидатор документов.</param>
        /// <param name="authorizationService">Сервис авторизации.</param>
        public DocumentController(IDocumentService documentService, IDocumentValidator documentValidator, IAuthorizationService authorizationService)
        {
            _documentService = documentService;
            _documentValidator = documentValidator;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Получить документ по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken token)
        {
            var model = await _documentService.GetByIdAsync(id, token);

            var perms = model?.Library is not null && await _authorizationService.HasAccessAsync(model.Library.Id, token);
            if (!perms)
                return Forbid();

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ActionName(nameof(PostAsync))]
        public async Task<IActionResult> PostAsync([FromForm] CreateDocumentDto model, IFormFile file, CancellationToken token)
        {
            if (!_documentValidator.TryValidateFile(file, out var metadata))
                return BadRequest();

            var perms = await _authorizationService.HasAccessAsync(model.LibraryId, AuthRolesDto.Writer, token);
            if (!perms)
                return Forbid();

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchAsync(Guid id, [FromForm] UpdateDocumentDto model, CancellationToken token)
        {
            var doc = await _documentService.GetByIdAsync(id, token);
            if (doc is null)
                return NotFound();

            var perms = doc.Library is not null && await _authorizationService.HasAccessAsync(doc.Library.Id, AuthRolesDto.Writer, token);
            if (!perms)
                return Forbid();

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken token)
        {
            var doc = await _documentService.GetByIdAsync(id, token);
            if (doc is null)
                return NotFound();

            var perms = doc.Library is not null && await _authorizationService.HasAccessAsync(doc.Library.Id, AuthRolesDto.Writer, token);
            if (!perms)
                return Forbid();

            await _documentService.DeleteAsync(id, token);
            return NoContent();
        }
    }
}
