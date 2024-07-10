using MyELib.Contracts.Document;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Utility;
using Riok.Mapperly.Abstractions;
using System.Linq.Expressions;
using DocumentEntity = MyELib.Domain.Document.Document;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.Document
{
    /// <inheritdoc cref="IDocumentMapper"/>
    [Mapper(UseReferenceHandling = true)]
    public partial class DocumentMapper : IDocumentMapper
    {
        [MapperIgnoreSource(nameof(DocumentEntity.LibraryId))]
        public partial DocumentDto MapToDto(DocumentEntity entity);
        public partial DocumentEntity MapToDocument(DocumentDto dto);
        public Expression<Func<DocumentEntity, bool>> MapToExpression(Expression<Func<DocumentDto, bool>> expression)
            => MapperUtils.MapToExpression<DocumentEntity, DocumentDto>(expression, MapToDto);

        public DocumentEntity MapToDocument(CreateDocumentDto dto, CreateDocumentDtoMetadata metadata)
        {
            var entity = MapToDocument(dto);
            MapToDocument(metadata, entity);
            return entity;
        }

        private partial DocumentEntity MapToDocument(CreateDocumentDto dto);
        private partial void MapToDocument(CreateDocumentDtoMetadata metadata, DocumentEntity entity);
    }
}
