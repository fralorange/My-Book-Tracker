using MyELib.Contracts.LibraryUser;
using LibraryUserEntity = MyELib.Domain.LibraryUser.LibraryUser;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.LibraryUser
{
    /// <summary>
    /// Маппер библиотек-пользователей.
    /// </summary>
    public interface ILibraryUserMapper
    {
        /// <summary>
        /// Преобразование сущности в ДТО.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <returns><see cref="LibraryUserDto"/></returns>
        LibraryUserDto MapToDto(LibraryUserEntity entity);
        /// <summary>
        /// Преобразование ДТО в сущность.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="LibraryUserEntity"/></returns>
        LibraryUserEntity MapToLibraryUser(LibraryUserDto dto);
        /// <summary>
        /// Преобразование создаваемой ДТО в сущность.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="LibraryUserEntity"/></returns>
        LibraryUserEntity MapToLibraryUser(CreateLibraryUserDto dto);
        /// <summary>
        /// Преобразование создаваемой ДТО в сущность.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="LibraryUserEntity"/></returns>
        LibraryUserEntity MapToLibraryUser(CreateCurrentLibraryUserDto dto);
        /// <summary>
        /// Преобразование обновляемой ДТО в сущность.
        /// </summary>
        /// <param name="dto">ДТО.</param>
        /// <returns><see cref="LibraryUserEntity"/></returns>
        LibraryUserEntity MapToLibraryUser(UpdateLibraryUserDto dto);
    }
}
