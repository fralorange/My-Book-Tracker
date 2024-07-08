using System.ComponentModel.DataAnnotations;

namespace MyELib.Contracts.User
{
    /// <summary>
    /// ДТО для аутентификации пользователя.
    /// </summary>
    public class LoginUserDto
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Username { get; set; }
        /// <summary>
        /// Пароль.
        /// </summary>
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
