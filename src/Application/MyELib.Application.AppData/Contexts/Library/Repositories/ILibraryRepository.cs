using System.Linq.Expressions;
using LibraryEntity = MyELib.Domain.Library.Library;

namespace MyELib.Application.AppData
{
    /// <summary>
    /// Репозиторий для работы с библиотеками.
    /// </summary>
    public interface ILibraryRepository
    {
        /// <summary>
        /// Возвращает коллекцию сущностей библиотек.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Коллекцию типа <see cref="LibraryEntity"/>.</returns>
        Task<IReadOnlyCollection<LibraryEntity>> GetAllAsync(CancellationToken token);
        /// <summary>
        /// Возвращает отфильтрованную коллекцию сущностей библиотек.
        /// </summary>
        /// <param name="predicate">Параметр фильтрации.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Коллекцию типа <see cref="LibraryEntity"/></returns>
        Task<IReadOnlyCollection<LibraryEntity>> GetAllFilteredAsync(Expression<Func<LibraryEntity, bool>> predicate, CancellationToken token);
        /// <summary>
        /// Возвращает сущность библиотеки по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Библиотеку типа <see cref="LibraryEntity"/>.</returns>
        Task<LibraryEntity?> GetByIdAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Создаёт сущность библиотеки.
        /// </summary>
        /// <param name="entity">Создаваемая сущность</param>
        /// <param name="token">Токен отмены операции.</param>
        Task<Guid> CreateAsync(LibraryEntity entity, CancellationToken token);
        /// <summary>
        /// Редактирует сущность библиотеки.
        /// </summary>
        /// <param name="entity">Отредактированная сущность.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task UpdateAsync(LibraryEntity entity, CancellationToken token);
        /// <summary>
        /// Удаляет сущность библиотеки.
        /// </summary>
        /// <param name="entity">Удаляемая сущность.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task DeleteAsync(LibraryEntity entity, CancellationToken token);
        /// <summary>
        /// Проверяет на сущность библиотеки.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid id, CancellationToken token);
    }
}