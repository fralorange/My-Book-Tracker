using MyELib.Contracts.User;
using System.Linq.Expressions;
using UserEntity = MyELib.Domain.User.User;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.User
{
    /// <summary>
    /// Маппер для пользователей.
    /// </summary>
    public interface IUserMapper
    {
        /// <summary>
        /// Преобразование сущности пользователя в ДТО.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns><see cref="UserDto"/></returns>
        UserDto MapToDto(UserEntity entity);
        /// <summary>
        /// Преобразование ДТО в сущность пользователя.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="UserEntity"/></returns>
        UserEntity MapToUser(UserDto dto);
        /// <summary>
        /// Преобразование создаваемой ДТО в сущность пользователя.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="UserEntity"/></returns>
        UserEntity MapToUser(CreateUserDto dto);
        /// <summary>
        /// Преобразование редактируемой ДТО в сущность пользователя.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="UserEntity"/></returns>
        UserEntity MapToUser(UpdateUserDto dto);
        /// <summary>
        /// Преобразование (оборачивание) предикаты ДТО в предикату сущности пользователя.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Expression<Func<UserEntity, bool>> MapToExpression(Expression<Func<UserDto, bool>> expression);
    }
}
