using MyELib.Contracts.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyELib.Contracts.LibraryUser
{
    /// <summary>
    /// ДТО для редактирования библиотеки-пользователя.
    /// </summary>
    public class UpdateLibraryUserDto
    {
        /// <summary>
        /// Роль пользователя в библиотеке.
        /// </summary>
        [Required]
        [AllowedValues(AuthRolesDto.Reader, AuthRolesDto.Writer)]
        public AuthRolesDto Role { get; set; }
    }
}
