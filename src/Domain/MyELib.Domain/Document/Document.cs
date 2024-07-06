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
        public string Name { get; set; }
        /// <summary>
        /// Тип загруженного документа.
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// Содержимое документа.
        /// </summary>
        public byte[] Content { get; set; }
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
        public string UploadedBy { get; set; }
        /// <summary>
        /// Навигационное поле на объект библиотеки <see cref="Library.Library"/>.
        /// </summary>
        public virtual Library.Library? Library { get; set; }
        /// <summary>
        /// Идентификатор привязанной библиотеки.
        /// </summary>
        public Guid? LibraryId { get; set; }
    }
}
