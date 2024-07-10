using MyELib.Contracts.Document;
using System.Linq.Expressions;
using DocumentEntity = MyELib.Domain.Document.Document;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.Document
{
    /// <summary>
    /// Маппер документов.
    /// </summary>
    public interface IDocumentMapper
    {
        /// <summary>
        /// Преобразование сущности в ДТО.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns><see cref="DocumentDto"/></returns>
        DocumentDto MapToDto(DocumentEntity entity);
        /// <summary>
        /// Преобразование ДТО в сущность.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="DocumentEntity"/></returns>
        DocumentEntity MapToDocument(DocumentDto dto);
        /// <summary>
        /// Преобразование (оборачивание) предикаты ДТО в предикату сущности.
        /// </summary>
        /// <param name="expression">Предиката.</param>
        Expression<Func<DocumentEntity, bool>> MapToExpression(Expression<Func<DocumentDto, bool>> expression);
        /// <summary>
        /// Преобразование ДТО и метаданных в сущность.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <param name="metadata">Метаданные.</param>
        /// <returns><see cref="DocumentEntity"/></returns>
        DocumentEntity MapToDocument(CreateDocumentDto dto, CreateDocumentDtoMetadata metadata);
    }
}
