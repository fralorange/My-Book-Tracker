using Microsoft.AspNetCore.Http;
using System.Text;
using Xceed.Words.NET;

namespace MyELib.Application.AppData.Contexts.Document.Validator.Handlers
{
    /// <summary>
    /// Обработчик DOC/DOCX
    /// </summary>
    public class DocHandler : DocumentHandler
    {
        /// <inheritdoc/>
        public override byte[]? Handle(IFormFile file, string fileType)
        {
            if (fileType.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || fileType.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
            {
                using var doc = DocX.Load(file.OpenReadStream());
                var paragraphs = doc.Paragraphs;
                var strBuilder = new StringBuilder();
                foreach (var paragraph in paragraphs)
                {
                    strBuilder.Append(paragraph.Text).Append(Environment.NewLine);
                }
                return Encoding.UTF8.GetBytes(strBuilder.ToString());
            }
            return base.Handle(file, fileType);
        }
    }
}
