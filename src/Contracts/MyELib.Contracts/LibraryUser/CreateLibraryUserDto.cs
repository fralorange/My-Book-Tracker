using MyELib.Contracts.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyELib.Contracts.LibraryUser
{
    /// <summary>
    /// ДТО для создания библиотеки-пользователя.
    /// </summary>
    public class CreateLibraryUserDto
    {
        /// <summary>
        /// Уникальный идентификатор на библиотеку.
        /// </summary>
        [Required]
        public Guid LibraryId { get; set; }
        /// <summary>
        /// Уникальный идентификатор на пользователя.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        /// <summary>
        /// Роль пользователя в библиотеке.
        /// </summary>
        [Required]
        public AuthRolesDto Role { get; set; }
    }
}
