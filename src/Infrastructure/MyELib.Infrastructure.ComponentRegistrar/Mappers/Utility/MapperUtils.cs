using System.Linq.Expressions;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.Utility
{
    public static class MapperUtils
    {
        public static Expression<Func<TSource, bool>> MapToExpression<TSource, TDestination>(
            Expression<Func<TDestination, bool>> expression,
            Func<TSource, TDestination> mapToDto)
        {
            var func = expression.Compile();
            var newFunc = new Func<TSource, bool>(ent => func(mapToDto(ent)));

            var param = Expression.Parameter(typeof(TSource), "entity");
            var body = Expression.Invoke(Expression.Constant(newFunc), param);

            return Expression.Lambda<Func<TSource, bool>>(body, param);
        }
    }
}
