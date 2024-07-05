using System.ComponentModel.DataAnnotations;

namespace MyELib.Contracts.Document
{
    /// <summary>
    /// ДТО для редактирования документа.
    /// </summary>
    public class UpdateDocumentDto
    {
        [MinLength(3)]
        [MaxLength(50)]
        public required string Name { get; set; }
    }
}
