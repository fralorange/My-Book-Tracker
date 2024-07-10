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
        /// Навигационное поле на коллекцию документов <see cref="Document.Document"/>
        /// </summary>
        public virtual IEnumerable<Document.Document>? Documents { get; set; }
        /// <summary>
        /// Навигационное поле на на коллекцию <see cref="LibraryUser.LibraryUser"/>
        /// </summary>
        public virtual IEnumerable<LibraryUser.LibraryUser> LibraryUsers { get; set; }
    }
}
