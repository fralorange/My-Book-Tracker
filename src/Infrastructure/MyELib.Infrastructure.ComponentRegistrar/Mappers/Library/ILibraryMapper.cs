using MyELib.Contracts.Library;
using System.Linq.Expressions;
using LibraryEntity = MyELib.Domain.Library.Library;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.Library
{
    /// <summary>
    /// Маппер для библиотек.
    /// </summary>
    public interface ILibraryMapper
    {
        /// <summary>
        /// Преобразование сущности в ДТО.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns><see cref="LibraryDto"/></returns>
        LibraryDto MapToDto(LibraryEntity entity);
        /// <summary>
        /// Преобразование создаваемой ДТО в сущность.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="LibraryEntity"/></returns>
        LibraryEntity MapToLibrary(CreateLibraryDto dto);
        /// <summary>
        /// Преобразование обновляемой ДТО в сущность.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="LibraryEntity"/></returns>
        LibraryEntity MapToLibrary(UpdateLibraryDto dto);
        /// <summary>
        /// Преобразование ДТО в сущность.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="LibraryEntity"/></returns>
        LibraryEntity MapToLibrary(LibraryDto dto);
        /// <summary>
        /// Преобразование (оборачивание) предикаты ДТО в предикату сущности.
        /// </summary>
        /// <param name="expression">Предиката.</param>
        Expression<Func<LibraryEntity, bool>> MapToExpression(Expression<Func<LibraryDto, bool>> expression);
    }
}
