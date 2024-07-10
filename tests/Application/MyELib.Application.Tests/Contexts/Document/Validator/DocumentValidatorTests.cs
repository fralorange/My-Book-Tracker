using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MyELib.Application.AppData.Contexts.Document.Validator;
using MyELib.Contracts.Document;

namespace MyELib.Application.Tests.Contexts.Document.Validator
{
    public class DocumentValidatorTests
    {
        [Fact]
        public void TryValidateFormFile_ReturnsTrueOutMetadata()
        {
            // Arrange
            var documentValidator = new DocumentValidator();

            var fileMock = new Mock<IFormFile>();
            fileMock.SetupGet(f => f.FileName).Returns("example.txt");
            fileMock.SetupGet(f => f.Length).Returns(17450);

            var date = DateTime.UtcNow;

            var fileBytes = new byte[17450];
            new Random().NextBytes(fileBytes);

            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(fileBytes));

            var newBytes = new byte[17450];
            Array.Copy(fileBytes, newBytes, fileBytes.Length);

            var expected = new CreateDocumentDtoMetadata
            {
                Content = newBytes,
                FileType = ".txt",
                Size = 17450,
                UploadedDate = date
            };

            // Act
            var isValid = documentValidator.TryValidateFile(fileMock.Object, out var actual);

            // Assert
            if (isValid)
            {
                actual.UploadedDate = date;
                actual.Should().BeEquivalentTo(expected);
            } else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void TryValidateFormFile_ReturnsFalseNullMetadata()
        {
            // Arrange
            var documentValidator = new DocumentValidator();

            var fileMock = new Mock<IFormFile>();
            fileMock.SetupGet(f => f.FileName).Returns("example.xlns");
            fileMock.SetupGet(f => f.Length).Returns(220563061);

            var fileBytes = new byte[220563061];
            new Random().NextBytes(fileBytes);

            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(fileBytes));

            var newBytes = new byte[220563061];
            Array.Copy(fileBytes, newBytes, fileBytes.Length);

            // Act
            var isValid = documentValidator.TryValidateFile(fileMock.Object, out var actual);
            var isNotValid = !isValid;

            // Assert
            Assert.True(isNotValid);
        }
    }
}
