using MyELib.Contracts.Library;
using Riok.Mapperly.Abstractions;
using System.Linq.Expressions;
using LibraryEntity = MyELib.Domain.Library.Library;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.Library
{
    /// <inheritdoc cref="ILibraryMapper"/>
    [Mapper(UseReferenceHandling = true)]
    public partial class LibraryMapper : ILibraryMapper
    {
        public partial LibraryDto MapToDto(LibraryEntity entity);
        public partial LibraryEntity MapToLibrary(CreateLibraryDto entity);
        public partial LibraryEntity MapToLibrary(UpdateLibraryDto entity);
        public partial LibraryEntity MapToLibrary(LibraryDto dto);
        public Expression<Func<LibraryEntity, bool>> MapToExpression(Expression<Func<LibraryDto, bool>> expression)
        {
            var func = expression.Compile();
            var newFunc = new Func<LibraryEntity, bool>(ent => func(MapToDto(ent)));

            var param = Expression.Parameter(typeof(LibraryEntity), "entity");
            var body = Expression.Invoke(Expression.Constant(newFunc), param);

            return Expression.Lambda<Func<LibraryEntity, bool>>(body, param);
        }
    }
}
