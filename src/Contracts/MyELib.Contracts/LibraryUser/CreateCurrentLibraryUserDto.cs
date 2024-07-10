using System.ComponentModel.DataAnnotations;

namespace MyELib.Contracts.LibraryUser
{
    /// <summary>
    /// ДТО для создания библиотеки-пользователя для текущего пользователя.
    /// </summary>
    public class CreateCurrentLibraryUserDto
    {
        /// <summary>
        /// Уникальный идентификатор на библиотеку.
        /// </summary>
        [Required]
        public Guid LibraryId { get; set; }
    }
}
