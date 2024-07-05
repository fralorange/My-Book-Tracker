using System.Linq.Expressions;
using DocumentEntity = MyELib.Domain.Document.Document;

namespace MyELib.Application.AppData.Contexts.Document.Repositories
{
    /// <summary>
    /// Репозиторий для работы с документами.
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        /// Возвращает коллекцию сущностей документов.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Коллекцию типа <see cref="DocumentEntity"/></returns>
        Task<IReadOnlyCollection<DocumentEntity>> GetAllAsync(CancellationToken token);
        /// <summary>
        /// Возвращает отфильтрованную коллекцию сущностей документов.
        /// </summary>
        /// <param name="expression">Параметр фильтрации.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Коллекцию типа <see cref="DocumentEntity"/></returns>
        Task<IReadOnlyCollection<DocumentEntity>> GetAllFilteredAsync(Expression<Func<DocumentEntity, bool>> expression, CancellationToken token);
        /// <summary>
        /// Возвращает сущность документа по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Документ типа <see cref="DocumentEntity"/></returns>
        Task<DocumentEntity?> GetByIdAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Создаёт сущность документа.
        /// </summary>
        /// <param name="entity">Создаваемый документ.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Уникальный идентфикатор типа <see cref="Guid"/></returns>
        Task<Guid> CreateAsync(DocumentEntity entity, CancellationToken token);
        /// <summary>
        /// Редактирует сущность документа.
        /// </summary>
        /// <param name="entity">Отредактированный документ.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task UpdateAsync(DocumentEntity entity, CancellationToken token);
        /// <summary>
        /// Удаляет сущность документа.
        /// </summary>
        /// <param name="entity">Удаляемая сущность.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task DeleteAsync(DocumentEntity entity, CancellationToken token);
    }
}
