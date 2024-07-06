using MyELib.Domain.Base;

namespace MyELib.Domain.Library
{
    /// <summary>
    /// Сущность библиотеки.
    /// </summary>
    public class Library : BaseEntity
    {
        /// <summary>
        /// Название библиотеки.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Навигационное поле на список документов <see cref="Document.Document"/>
        /// </summary>
        public List<Document.Document>? Documents { get; set; }
    }
}
