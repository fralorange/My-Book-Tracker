using Microsoft.AspNetCore.Http;
using MyELib.Contracts.Document;
using System.ComponentModel.DataAnnotations;
using DataValidator = System.ComponentModel.DataAnnotations.Validator;

namespace MyELib.Application.AppData.Contexts.Document.Validator
{
    /// <inheritdoc cref="IDocumentValidator"/>
    public class DocumentValidator : IDocumentValidator
    {
        public bool TryValidateFile(IFormFile file, out CreateDocumentDtoMetadata metadata)
        {
            var exception = ValidateInternal(file, out metadata);
            return exception is null;
        }

        private Exception? ValidateInternal(IFormFile file, out CreateDocumentDtoMetadata metadata)
        {
            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                var byteArrays = binaryReader.ReadBytes((int)file.Length);

                metadata = new CreateDocumentDtoMetadata()
                {
                    Content = byteArrays,
                    FileType = Path.GetExtension(file.FileName).ToLower(),
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
}
