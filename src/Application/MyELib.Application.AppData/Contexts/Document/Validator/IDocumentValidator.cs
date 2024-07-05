using Microsoft.AspNetCore.Http;
using MyELib.Contracts.Document;

namespace MyELib.Application.AppData.Contexts.Document.Validator
{
    /// <summary>
    /// Валидатор для проверки метаданных документов
    /// </summary>
    public interface IDocumentValidator
    {
        /// <summary>
        /// Проверяет файл на соответствие требованиям и извлекает метаданные.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="metadata">Извлекаемые метаданные.</param>
        /// <returns>True, если файл прошел проверку; иначе false</returns>
        bool TryValidateFile(IFormFile file, out CreateDocumentDtoMetadata metadata);
    }
}
