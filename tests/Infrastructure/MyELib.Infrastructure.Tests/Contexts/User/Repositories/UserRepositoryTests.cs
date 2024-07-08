using FluentAssertions;
using Moq;
using MyELib.Application.AppData.Contexts.User.Repositories;
using MyELib.Infrastructure.DataAccess.Contexts.User.Repositories;
using MyELib.Infrastructure.Repository;
using System.Linq.Expressions;
using UserEntity = MyELib.Domain.User.User;

namespace MyELib.Infrastructure.Tests.Contexts.User.Repositories
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task GetUsers_ReturnAllUsers()
        {
            // Arrange
            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 1", Email = "Юзер@mail.ru" };
            var user2 = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 2", Email = "Юзер@mail.ru" };

            var repositoryMock = new Mock<IRepository<UserEntity>>();
            repositoryMock.Setup(r => r.GetAll()).Returns(new[] { user1, user2 }.AsQueryable());

            IUserRepository userRepository = new UserRepository(repositoryMock.Object);

            var expected = (IReadOnlyCollection<UserEntity>)[user1, user2];

            // Act
            var actual = await userRepository.GetAllAsync(CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetFilteredUsers_ReturnAllFilteredUsers()
        {
            // Arrange
            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 1", Email = "Юзер@mail.ru" };
            var user2 = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 2", Email = "Юзер@mail.ru" };
            var user3 = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 3", Email = "Юзер@mail.ru" };
            var user4 = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 4", Email = "Юзер@mail.ru" };

            var repositoryMock = new Mock<IRepository<UserEntity>>();
            repositoryMock
                .Setup(r => r.GetAllFiltered(It.Is<Expression<Func<UserEntity, bool>>>(e => e.Compile()(user1) && e.Compile()(user3))))
                .Returns(new[] { user1, user3 }.AsQueryable());

            IUserRepository userRepository = new UserRepository(repositoryMock.Object);

            Expression<Func<UserEntity, bool>> expression = entity => entity.Username.EndsWith('1') || entity.Username.EndsWith('3');

            var expected = (IReadOnlyCollection<UserEntity>)[user1, user2, user3, user4];
            expected = expected.Where(expression.Compile()).ToArray();

            // Act
            var actual = await userRepository.GetAllFilteredAsync(expression, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetUsersById_ReturnCorrectUser()
        {
            // Arrange
            var expected = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 1", Email = "Юзер@mail.ru" };

            var repositoryMock = new Mock<IRepository<UserEntity>>();
            repositoryMock.Setup(r => r.GetByIdAsync(expected.Id, CancellationToken.None))
                .ReturnsAsync(expected);

            IUserRepository userRepository = new UserRepository(repositoryMock.Object);

            // Act
            var actual = await userRepository.GetByIdAsync(expected.Id, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateUser_UserCreatedSuccessfully()
        {
            // Arrange
            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 1", Email = "Юзер@mail.ru" };

            var repositoryMock = new Mock<IRepository<UserEntity>>();
            repositoryMock.Setup(r => r.GetAll())
                .Returns(new[] { user1 }.AsQueryable());

            IUserRepository userRepository = new UserRepository(repositoryMock.Object);

            // Act
            var actual = await userRepository.CreateAsync(user1, CancellationToken.None);

            // Assert
            Assert.Contains(user1, await userRepository.GetAllAsync(CancellationToken.None));
        }

        [Fact]
        public async Task UpdateUser_UserUpdateSuccessfully()
        {
            // Arrange
            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 1", Email = "Юзер@mail.ru" };
            var updatedUser1 = new UserEntity { Id = Guid.NewGuid(), Username = "Новый Юзер 1", Email = "Юзер@mail.ru" };

            var repositoryMock = new Mock<IRepository<UserEntity>>();

            IUserRepository userRepository = new UserRepository(repositoryMock.Object);

            // Act
            await userRepository.UpdateAsync(updatedUser1, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.UpdateAsync(updatedUser1, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_UserDeletedSuccessfully()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository<UserEntity>>();

            IUserRepository userRepository = new UserRepository(repositoryMock.Object);

            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 1", Email = "Юзер@mail.ru" };

            // Act
            await userRepository.DeleteAsync(new UserEntity { Id = user1.Id, Username = user1.Username }, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.DeleteAsync(It.Is<UserEntity>(lib => lib.Id == lib.Id), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task UserExists_UserExistsTrue()
        {
            // Arrange
            var user1 = new UserEntity { Id = Guid.NewGuid(), Username = "Юзер 1", Email = "Юзер@mail.ru" };

            var targetId = user1.Id;

            var repositoryMock = new Mock<IRepository<UserEntity>>();
            repositoryMock.Setup(r => r.ExistsAsync(targetId, CancellationToken.None)).ReturnsAsync(true);

            IUserRepository userRepository = new UserRepository(repositoryMock.Object);

            // Act
            var result = await userRepository.ExistsAsync(targetId, CancellationToken.None);

            // Assert
            Assert.True(result);
        }
    }
}
