using Microsoft.AspNetCore.Http;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
using UglyToad.PdfPig.Geometry;

namespace MyELib.Application.AppData.Contexts.Document.Validator.Handlers
{
    /// <summary>
    /// Обработчик файлов PDF.
    /// </summary>
    public class PdfHandler : DocumentHandler
    {
        /// <inheritdoc/>
        public override byte[]? Handle(IFormFile file, string fileType)
        {
            if (fileType.Equals(".pdf", StringComparison.CurrentCultureIgnoreCase))
            {
                using var pdfDocument = PdfDocument.Open(file.OpenReadStream());
                var textBuilder = new StringBuilder();
                for (var i = 0; i < pdfDocument.NumberOfPages; i++)
                {
                    var page = pdfDocument.GetPage(i + 1);

                    var words = page.GetWords(NearestNeighbourWordExtractor.Instance);
                    var blocks = DocstrumBoundingBoxes.Instance.GetBlocks(words);

                    var unsupervisedReadingOrderDetector = new UnsupervisedReadingOrderDetector(10);
                    var orderedBlocks = unsupervisedReadingOrderDetector.Get(blocks);

                    foreach (var block in orderedBlocks)
                    {
                        var wordsList = words.Where(x => x.BoundingBox.IntersectsWith(block.BoundingBox)).ToList();
                        foreach (var word in wordsList.Where(word => !string.IsNullOrWhiteSpace(word.Text)))
                        {
                            textBuilder.Append(word.Text).Append(' ');
                        }
                    }
                }
                return Encoding.UTF8.GetBytes(textBuilder.ToString());
            }
            return base.Handle(file, fileType);
        }
    }
}
