using MyELib.Contracts.Identity;

namespace MyELib.Contracts.LibraryUser
{
    /// <summary>
    /// ДТО библиотеки-пользователя.
    /// </summary>
    public class LibraryUserDto : Base.BaseDto
    {
        /// <summary>
        /// Уникальный идентификатор на библиотеку.
        /// </summary>
        public Guid LibraryId { get; set; }
        /// <summary>
        /// Уникальный идентификатор на пользователя.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Роль пользователя в библиотеке.
        /// </summary>
        public AuthRolesDto Role { get; set; }
    }
}
