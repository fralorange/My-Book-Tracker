using MyELib.Domain.Identity;

namespace MyELib.Domain.LibraryUser
{
    /// <summary>
    /// Связующий домен библиотеки и пользователя.
    /// </summary>
    public class LibraryUser : Base.BaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор на библиотеку.
        /// </summary>
        public Guid LibraryId { get; set; }
        /// <summary>
        /// Навигационное поле <see cref="Library.Library"/>.
        /// </summary>
        public virtual Library.Library Library { get; set; }
        /// <summary>
        /// Уникальный идентификатор на пользователя.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Навигационное поле <see cref="User.User"/>.
        /// </summary>
        public virtual User.User User { get; set; }
        /// <summary>
        /// Роль пользователя в библиотеке.
        /// </summary>
        public AuthRoles Role { get; set; }
    }
}
