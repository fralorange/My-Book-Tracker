using Microsoft.AspNetCore.Http;

namespace MyELib.Application.AppData.Contexts.Document.Validator.Handlers
{
    /// <summary>
    /// <inheritdoc cref="IDocumentHandler"/> (Базовый)
    /// </summary>
    public abstract class DocumentHandler : IDocumentHandler
    {
        /// <summary>
        /// Следующий элемент.
        /// </summary>
        protected IDocumentHandler? next;

        /// <inheritdoc/>
        public void SetNext(IDocumentHandler handler) => next = handler;

        /// <summary>
        /// Обрабатывает файл.
        /// </summary>
        /// <param name="file">Обрабатываемый файл.</param>
        /// <param name="fileType">Тип файла.</param>
        /// <returns></returns>
        public virtual byte[]? Handle(IFormFile file, string fileType)
        {
            return next?.Handle(file, fileType);
        }
    }
}
