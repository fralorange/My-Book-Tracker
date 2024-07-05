using MyELib.Contracts.Document;
using System.Linq.Expressions;

namespace MyELib.Application.AppData.Contexts.Document.Services
{
    /// <summary>
    /// Сервис для работы с документами.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Возвращает коллекцию документов.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Коллекцию типа <see cref="DocumentDto"/></returns>
        Task<IReadOnlyCollection<DocumentDto>> GetAllAsync(CancellationToken token);
        /// <summary>
        /// Возвращает отфильтрованную коллекцию документов.
        /// </summary>
        /// <param name="expression">Параметр фильтрации.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Отфильтрованную коллекцию типа <see cref="DocumentDto"/></returns>
        Task<IReadOnlyCollection<DocumentDto>> GetAllFilteredAsync(Expression<Func<DocumentDto, bool>> expression, CancellationToken token);
        /// <summary>
        /// Возвращает документ по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Документ типа <see cref="DocumentDto"/></returns>
        Task<DocumentDto?> GetByIdAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Создаёт документ.
        /// </summary>
        /// <param name="dto">Создаваемый документ.</param>
        /// <param name="metadata">Метадата создаваемого документа.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Уникальный идентификатор типа <see cref="Guid"/></returns>
        Task<Guid> CreateAsync(CreateDocumentDto dto, CreateDocumentDtoMetadata metadata, CancellationToken token);
        /// <summary>
        /// Редактирует документ.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="dto">Отредактированный документ.</param>
        /// <param name="token">Токен отмены операциию</param>
        Task PatchAsync(Guid id, UpdateDocumentDto dto, CancellationToken token);
        /// <summary>
        /// Удаляет документ.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task DeleteAsync(Guid id, CancellationToken token);
    }
}
