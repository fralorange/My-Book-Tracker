using MyELib.Domain.Base;

namespace MyELib.Domain.Document
{
    /// <summary>
    /// Сущность документа.
    /// </summary>
    public class Document : BaseEntity
    {
        /// <summary>
        /// Название документа.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Тип загруженного документа.
        /// </summary>
        public required string FileType { get; set; }
        /// <summary>
        /// Содержимое документа.
        /// </summary>
        public required byte[] Content { get; set; }
        /// <summary>
        /// Размер документа.
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Дата загрузки документа.
        /// </summary>
        public DateTime UploadedDate { get; set; }
        /// <summary>
        /// Автор загрузки документа.
        /// </summary>
        public required string UploadedBy { get; set; }
        /// <summary>
        /// Навигационное поле на объект библиотеки <see cref="Library.Library"/>.
        /// </summary>
        public required Library.Library Library { get; set; }
    }
}
