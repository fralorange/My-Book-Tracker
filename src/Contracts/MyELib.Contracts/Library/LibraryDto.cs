using MyELib.Contracts.Base;

namespace MyELib.Contracts.Library
{
    /// <summary>
    /// ДТО библиотеки.
    /// </summary>
    public class LibraryDto : BaseDto
    {
        /// <summary>
        /// Название библиотеки.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Документы.
        /// </summary>
        public IReadOnlyCollection<Document.DocumentDto>? Documents { get; set; }
    }
}
