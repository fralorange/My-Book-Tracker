namespace MyELib.Domain.User
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User : Base.BaseEntity
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
        /// Хешированный пароль.
        /// </summary>
        public string HashedPassword { get; set; }
        /// <summary>
        /// Соль хешированного поля.
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// Навигационное поле на коллекцию <see cref="LibraryUser.LibraryUser"/>
        /// </summary>
        public virtual IEnumerable<LibraryUser.LibraryUser>? LibraryUsers { get; set; }
    }
}
