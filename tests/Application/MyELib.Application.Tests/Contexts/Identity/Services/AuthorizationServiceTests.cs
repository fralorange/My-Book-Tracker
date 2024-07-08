using Moq;
using MyELib.Application.AppData.Contexts.Identity.Services;
using MyELib.Application.AppData.Contexts.Library.Services;
using MyELib.Application.AppData.Contexts.User.Services;
using MyELib.Contracts.Document;
using MyELib.Contracts.Identity;
using MyELib.Contracts.Library;
using MyELib.Contracts.LibraryUser;
using MyELib.Contracts.User;

namespace MyELib.Application.Tests.Contexts.Identity.Services
{
    public class AuthorizationServiceTests
    {
        [Fact]
        public async Task UserHasAccessToLibrary_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();

            var libraryuser1 = new LibraryUserDto { Id = Guid.NewGuid(), LibraryId = libraryId, UserId = userId, Role = AuthRolesDto.Owner };

            var currentUser = new UserDto 
            { 
                Id = userId, 
                Email = "qwerty123@gmail.com", 
                Username = "qwerty123", 
                LibraryUsers = [libraryuser1] };

            var libraryServiceMock = new Mock<ILibraryService>();

            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(us => us.GetCurrentUser(CancellationToken.None))
                .ReturnsAsync(currentUser);

            var service = new AuthorizationService(userServiceMock.Object, libraryServiceMock.Object);

            // Act
            var result = await service.HasAccessAsync(libraryId, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserHasAccessToLibrary_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();

            var libraryuser1 = new LibraryUserDto { Id = Guid.NewGuid(), LibraryId = libraryId, UserId = Guid.NewGuid(), Role = AuthRolesDto.Owner };

            var currentUser = new UserDto
            {
                Id = userId,
                Email = "qwerty123@gmail.com",
                Username = "qwerty123",
                LibraryUsers = [libraryuser1]
            };

            var libraryServiceMock = new Mock<ILibraryService>();

            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(us => us.GetCurrentUser(CancellationToken.None))
                .ReturnsAsync(currentUser);

            var service = new AuthorizationService(userServiceMock.Object, libraryServiceMock.Object);

            // Act
            var result = await service.HasAccessAsync(libraryId, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UserHasAccessToLibraryThroughDocument_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();
            var docId = Guid.NewGuid();

            var libraryuser1 = new LibraryUserDto { Id = Guid.NewGuid(), LibraryId = libraryId, UserId = userId, Role = AuthRolesDto.Owner };

            var currentUser = new UserDto
            {
                Id = userId,
                Email = "qwerty123@gmail.com",
                Username = "qwerty123",
                LibraryUsers = [libraryuser1]
            };

            var lib1 = new LibraryDto
            {
                Id = libraryId,
                Name = "Библиотека 1",
            };

            var doc1 = new DocumentDto
            {
                Id = docId,
                Name = "Книга 1",
                FileType = ".pdf",
                Content = [],
                Size = 10000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = lib1
            };
            lib1.Documents = [doc1];

            var libraryServiceMock = new Mock<ILibraryService>();

            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(us => us.GetCurrentUser(CancellationToken.None))
                .ReturnsAsync(currentUser);

            var service = new AuthorizationService(userServiceMock.Object, libraryServiceMock.Object);

            // Act
            var result = await service.HasAccessAsync(doc1.Library.Id, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserHasAccessToLibraryThroughDocument_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();
            var docId = Guid.NewGuid();

            var libraryuser1 = new LibraryUserDto { Id = Guid.NewGuid(), LibraryId = Guid.NewGuid(), UserId = userId, Role = AuthRolesDto.Owner };

            var currentUser = new UserDto
            {
                Id = userId,
                Email = "qwerty123@gmail.com",
                Username = "qwerty123",
                LibraryUsers = [libraryuser1]
            };

            var lib1 = new LibraryDto
            {
                Id = libraryId,
                Name = "Библиотека 1",
            };

            var doc1 = new DocumentDto
            {
                Id = docId,
                Name = "Книга 1",
                FileType = ".pdf",
                Content = [],
                Size = 10000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = lib1
            };
            lib1.Documents = [doc1];

            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(us => us.GetCurrentUser(CancellationToken.None))
                .ReturnsAsync(currentUser);

            var libraryServiceMock = new Mock<ILibraryService>();

            var service = new AuthorizationService(userServiceMock.Object, libraryServiceMock.Object);

            // Act
            var result = await service.HasAccessAsync(doc1.Library.Id, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UserHasAccessToLibraryWithRole_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();

            var libraryuser1 = new LibraryUserDto { Id = Guid.NewGuid(), LibraryId = libraryId, UserId = userId, Role = AuthRolesDto.Writer };

            var currentUser = new UserDto
            {
                Id = userId,
                Email = "qwerty123@gmail.com",
                Username = "qwerty123",
                LibraryUsers = [libraryuser1]
            };

            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(us => us.GetCurrentUser(CancellationToken.None))
                .ReturnsAsync(currentUser);

            var libraryServiceMock = new Mock<ILibraryService>();

            var service = new AuthorizationService(userServiceMock.Object, libraryServiceMock.Object);

            // Act
            var result = await service.HasAccessAsync(libraryId, AuthRolesDto.Reader, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserHasAccessToLibraryWithRole_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();

            var libraryuser1 = new LibraryUserDto { Id = Guid.NewGuid(), LibraryId = libraryId, UserId = userId, Role = AuthRolesDto.Writer };

            var currentUser = new UserDto
            {
                Id = userId,
                Email = "qwerty123@gmail.com",
                Username = "qwerty123",
                LibraryUsers = [libraryuser1]
            };

            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(us => us.GetCurrentUser(CancellationToken.None))
                .ReturnsAsync(currentUser);

            var libraryServiceMock = new Mock<ILibraryService>();

            var service = new AuthorizationService(userServiceMock.Object, libraryServiceMock.Object);

            // Act
            var result = await service.HasAccessAsync(libraryId, AuthRolesDto.Owner, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UserHasAccessToLibraryWithRoleThroughDocuments_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();
            var docId = Guid.NewGuid();

            var libraryuser1 = new LibraryUserDto { Id = Guid.NewGuid(), LibraryId = libraryId, UserId = userId, Role = AuthRolesDto.Writer };

            var currentUser = new UserDto
            {
                Id = userId,
                Email = "qwerty123@gmail.com",
                Username = "qwerty123",
                LibraryUsers = [libraryuser1]
            };

            var lib1 = new LibraryDto
            {
                Id = libraryId,
                Name = "Библиотека 1",
            };

            var doc1 = new DocumentDto
            {
                Id = docId,
                Name = "Книга 1",
                FileType = ".pdf",
                Content = [],
                Size = 10000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = lib1
            };
            lib1.Documents = [doc1];

            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(us => us.GetCurrentUser(CancellationToken.None))
                .ReturnsAsync(currentUser);

            var libraryServiceMock = new Mock<ILibraryService>();

            var service = new AuthorizationService(userServiceMock.Object, libraryServiceMock.Object);

            // Act
            var result = await service.HasAccessAsync(doc1.Library.Id, AuthRolesDto.Reader, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserHasAccessToLibraryWithRoleThroughDocuments_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();
            var docId = Guid.NewGuid();

            var libraryuser1 = new LibraryUserDto { Id = Guid.NewGuid(), LibraryId = Guid.NewGuid(), UserId = userId, Role = AuthRolesDto.Reader };

            var currentUser = new UserDto
            {
                Id = userId,
                Email = "qwerty123@gmail.com",
                Username = "qwerty123",
                LibraryUsers = [libraryuser1]
            };

            var lib1 = new LibraryDto
            {
                Id = libraryId,
                Name = "Библиотека 1",
            };

            var doc1 = new DocumentDto
            {
                Id = docId,
                Name = "Книга 1",
                FileType = ".pdf",
                Content = [],
                Size = 10000,
                UploadedBy = "123",
                UploadedDate = DateTime.Now,
                Library = lib1
            };
            lib1.Documents = [doc1];

            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(us => us.GetCurrentUser(CancellationToken.None))
                .ReturnsAsync(currentUser);

            var libraryServiceMock = new Mock<ILibraryService>();

            var service = new AuthorizationService(userServiceMock.Object, libraryServiceMock.Object);

            // Act
            var result = await service.HasAccessAsync(doc1.Library.Id, AuthRolesDto.Reader, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsLibraryUserless_ReturnTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();

            var currentUser = new UserDto
            {
                Id = userId,
                Email = "qwerty123@gmail.com",
                Username = "qwerty123",
                LibraryUsers = []
            };

            var library = new LibraryDto
            {
                Name = "Библиотека",
                LibraryUsers = []
            };

            var libraryServiceMock = new Mock<ILibraryService>();
            libraryServiceMock
                .Setup(ls => ls.GetByIdAsync(libraryId, CancellationToken.None))
                .ReturnsAsync(library);

            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(us => us.GetCurrentUser(CancellationToken.None))
                .ReturnsAsync(currentUser);

            var service = new AuthorizationService(userServiceMock.Object, libraryServiceMock.Object);

            // Act
            var result = await service.IsUserlessAsync(libraryId, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsLibraryUserless_ReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();

            var libraryuser1 = new LibraryUserDto { Id = Guid.NewGuid(), LibraryId = libraryId, UserId = userId, Role = AuthRolesDto.Owner };
            var libraryuser2 = new LibraryUserDto { Id = Guid.NewGuid(), LibraryId = libraryId, UserId = userId, Role = AuthRolesDto.Reader };

            var currentUser = new UserDto
            {
                Id = userId,
                Email = "qwerty123@gmail.com",
                Username = "qwerty123",
                LibraryUsers = []
            };

            var library = new LibraryDto
            {
                Name = "Библиотека",
                LibraryUsers = [libraryuser1, libraryuser2]
            };

            var libraryServiceMock = new Mock<ILibraryService>();
            libraryServiceMock
                .Setup(ls => ls.GetByIdAsync(libraryId, CancellationToken.None))
                .ReturnsAsync(library);

            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(us => us.GetCurrentUser(CancellationToken.None))
                .ReturnsAsync(currentUser);

            var service = new AuthorizationService(userServiceMock.Object, libraryServiceMock.Object);

            // Act
            var result = await service.IsUserlessAsync(libraryId, CancellationToken.None);

            // Assert
            Assert.False(result);
        }
    }
}
