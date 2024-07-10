using System.ComponentModel.DataAnnotations;

namespace MyELib.Contracts.Document
{
    /// <summary>
    /// Метадата создаваемого ДТО документа.
    /// </summary>
    public class CreateDocumentDtoMetadata
    {
        /// <summary>
        /// Тип файла.
        /// </summary>
        [AllowedValues([".pdf", ".doc", ".docx", ".txt"])]
        public required string FileType { get; set; }
        /// <summary>
        /// Контент файла.
        /// </summary>
        public required byte[] Content { get; set; }
        /// <summary>
        /// Размер файла.
        /// </summary>
        [Range(1, 20000000)]
        [Required]
        public long Size { get; set; }
        /// <summary>
        /// Дата загрузки.
        /// </summary>
        [Required]
        public DateTime UploadedDate { get; set; }
    }
}
