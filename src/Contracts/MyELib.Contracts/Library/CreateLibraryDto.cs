using System.ComponentModel.DataAnnotations;

namespace MyELib.Contracts.Library
{
    /// <summary>
    /// ДТО для создания библиотеки.
    /// </summary>
    public class CreateLibraryDto
    {
        /// <summary>
        /// Название библиотеки.
        /// </summary>
        [MinLength(3)]
        [MaxLength(100)]
        public required string Name { get; set; }
        /// <summary>
        /// Документы.
        /// </summary>
        public IReadOnlyCollection<Document.DocumentDto>? Documents { get; set; }
    }
}
