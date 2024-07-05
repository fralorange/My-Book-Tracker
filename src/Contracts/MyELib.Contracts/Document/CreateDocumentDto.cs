using System.ComponentModel.DataAnnotations;

namespace MyELib.Contracts.Document
{
    /// <summary>
    /// ДТО для создания докуметов.
    /// </summary>
    public class CreateDocumentDto
    {
        /// <summary>
        /// Название документа.
        /// </summary>
        [MinLength(3)]
        [MaxLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// Идентификатор библиотеки.
        /// </summary>
        public Guid? LibraryId { get; set; }
    }
}
