using Microsoft.AspNetCore.Http;

namespace MyELib.Application.AppData.Contexts.Document.Validator.Handlers
{
    /// <summary>
    /// Обработчик документов.
    /// </summary>
    public interface IDocumentHandler
    {
        /// <summary>
        /// Поставить следующим в цепочку обработки.
        /// </summary>
        /// <param name="handler">Обработчик.</param>
        void SetNext(IDocumentHandler handler);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">Обрабатываемый файл.</param>
        /// <param name="fileType">Тип файла.</param>
        /// <returns></returns>
        byte[]? Handle(IFormFile file, string fileType);
    }
}
