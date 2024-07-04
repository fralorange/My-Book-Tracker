using MyELib.Contracts.Document;
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
    }
}
