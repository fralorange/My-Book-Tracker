using System.ComponentModel.DataAnnotations;

namespace MyELib.Contracts.User
{
    /// <summary>
    /// ДТО для создания пользователя.
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Username { get; set; }
        /// <summary>
        /// Электронная почта.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Пароль.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
