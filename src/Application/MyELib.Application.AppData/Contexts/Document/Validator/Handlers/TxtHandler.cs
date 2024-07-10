using Microsoft.AspNetCore.Http;

namespace MyELib.Application.AppData.Contexts.Document.Validator.Handlers
{
    /// <summary>
    ///  Обработчик TXT
    /// </summary>
    public class TxtHandler : DocumentHandler
    {
        /// <inheritdoc/>
        public override byte[]? Handle(IFormFile file, string fileType)
        {
            if (fileType.Equals(".txt", StringComparison.CurrentCultureIgnoreCase))
            {
                using var binaryReader = new BinaryReader(file.OpenReadStream());
                var byteArrays = binaryReader.ReadBytes((int)file.Length);
                return byteArrays;
            }
            return base.Handle(file, fileType);
        }
    }
}
