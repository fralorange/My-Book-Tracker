using MyELib.Contracts.Library;
using System.Linq.Expressions;

namespace MyELib.Application.AppData.Contexts.Library.Services
{
    /// <summary>
    /// Сервис для работы с библиотеками.
    /// </summary>
    public interface ILibraryService
    {
        /// <summary>
        /// Возвращает коллекцию библиотек.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Коллекцию типа <see cref="LibraryDto"/>.</returns>
        Task<IReadOnlyCollection<LibraryDto>> GetAllAsync(CancellationToken token);
        /// <summary>
        /// Возвращает отфильтрованную коллекцию библиотек.
        /// </summary>
        /// <param name="expression">Параметр фильтрации.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Отфильтрованную коллекцию типа <see cref="LibraryDto"/>.</returns>
        Task<IReadOnlyCollection<LibraryDto>> GetAllFilteredAsync(Expression<Func<LibraryDto, bool>> expression, CancellationToken token);
        /// <summary>
        /// Возвращает библиотеку по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Библиотеку типа <see cref="LibraryDto"/>.</returns>
        Task<LibraryDto?> GetByIdAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Создаёт библиотеку.
        /// </summary>
        /// <param name="dto">Создаваемая библиотека.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task<Guid> CreateAsync(CreateLibraryDto dto, CancellationToken token);
        /// <summary>
        /// Редактирует библиотеку.
        /// </summary>
        /// <param name="dto">Отредактированная библиотека.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task UpdateAsync(UpdateLibraryDto dto, Guid id, CancellationToken token);
        /// <summary>
        /// Удаляет библиотеку.
        /// </summary>
        /// <param name="id">Уникальный идетификатор удаляемой библиотеки.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task DeleteAsync(Guid id, CancellationToken token);
    }
}
