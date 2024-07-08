using MyELib.Contracts.Base;

namespace MyELib.Contracts.User
{
    /// <summary>
    /// ДТО пользователя.
    /// </summary>
    public class UserDto : BaseDto
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Электронная почта.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Коллекция <see cref="LibraryUser.LibraryUserDto"/>.
        /// </summary>
        public virtual IEnumerable<LibraryUser.LibraryUserDto>? LibraryUsers { get; set; }
    }
}
