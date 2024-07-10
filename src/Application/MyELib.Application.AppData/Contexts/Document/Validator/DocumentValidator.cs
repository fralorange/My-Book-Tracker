using Microsoft.AspNetCore.Http;
using MyELib.Application.AppData.Contexts.Document.Validator.Handlers;
using MyELib.Contracts.Document;
using System.ComponentModel.DataAnnotations;
using DataValidator = System.ComponentModel.DataAnnotations.Validator;

namespace MyELib.Application.AppData.Contexts.Document.Validator
{
    /// <inheritdoc cref="IDocumentValidator"/>
    public class DocumentValidator : IDocumentValidator
    {
        private readonly IDocumentHandler _documentHandler;

        /// <summary>
        /// Инициализирует валидатор документов.
        /// </summary>
        public DocumentValidator()
        {
            _documentHandler = new PdfHandler();

            var docHandler = new DocHandler();
            var txtHandler = new TxtHandler();

            _documentHandler.SetNext(docHandler);
            docHandler.SetNext(txtHandler);
        }

        /// <inheritdoc/>
        public bool TryValidateFile(IFormFile file, out CreateDocumentDtoMetadata metadata)
        {
            var exception = ValidateInternal(file, out metadata);
            return exception is null;
        }

        private Exception? ValidateInternal(IFormFile file, out CreateDocumentDtoMetadata metadata)
        {
            var fileType = Path.GetExtension(file.FileName).ToLower();

            var content = _documentHandler.Handle(file, fileType);

            metadata = new CreateDocumentDtoMetadata()
            {
                Content = content,
                FileType = fileType,
                Size = file.Length,
                UploadedDate = DateTime.UtcNow,
            };

            var context = new ValidationContext(metadata);

            if (!DataValidator.TryValidateObject(metadata, context, null, true))
                return new ValidationException(nameof(CreateDocumentDtoMetadata));
            return null;
        }
    }
}
