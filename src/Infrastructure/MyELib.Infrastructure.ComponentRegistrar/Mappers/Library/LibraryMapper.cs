using MyELib.Contracts.Library;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Utility;
using Riok.Mapperly.Abstractions;
using System.Linq.Expressions;
using LibraryEntity = MyELib.Domain.Library.Library;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.Library
{
    /// <inheritdoc cref="ILibraryMapper"/>
    [Mapper(UseReferenceHandling = true)]
    public partial class LibraryMapper : ILibraryMapper
    {
        [MapperIgnoreSource(nameof(LibraryEntity.DocumentIds))]
        public partial LibraryDto MapToDto(LibraryEntity entity);
        [MapperIgnoreTarget(nameof(LibraryEntity.Id))]
        [MapperIgnoreTarget(nameof(LibraryEntity.Documents))]
        public partial LibraryEntity MapToLibrary(CreateLibraryDto entity);
        [MapperIgnoreTarget(nameof(LibraryEntity.Id))]
        [MapperIgnoreTarget(nameof(LibraryEntity.Documents))]
        public partial LibraryEntity MapToLibrary(UpdateLibraryDto entity);
        [MapperIgnoreTarget(nameof(LibraryEntity.DocumentIds))]
        public partial LibraryEntity MapToLibrary(LibraryDto dto);
        public Expression<Func<LibraryEntity, bool>> MapToExpression(Expression<Func<LibraryDto, bool>> expression)
            => MapperUtils.MapToExpression<LibraryEntity, LibraryDto>(expression, MapToDto);
    }
}
