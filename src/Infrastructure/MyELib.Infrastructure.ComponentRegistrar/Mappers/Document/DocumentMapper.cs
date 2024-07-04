using MyELib.Contracts.Document;
using Riok.Mapperly.Abstractions;
using DocumentEntity = MyELib.Domain.Document.Document;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.Document
{
    /// <inheritdoc cref="IDocumentMapper"/>
    [Mapper(UseReferenceHandling = true)]
    public partial class DocumentMapper : IDocumentMapper
    {
        public partial DocumentDto MapToDto(DocumentEntity entity);
        public partial DocumentEntity MapToDocument(DocumentDto dto);
    }
}
