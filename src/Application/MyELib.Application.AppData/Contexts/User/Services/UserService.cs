using Microsoft.AspNetCore.Http;
using MyELib.Application.AppData.Contexts.User.Repositories;
using MyELib.Contracts.User;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.User;
using System.Linq.Expressions;
using System.Security.Claims;

namespace MyELib.Application.AppData.Contexts.User.Services
{
    /// <inheritdoc cref="IUserService"/>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserMapper _userMapper;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Инициализирует сервис пользователей.
        /// </summary>
        public UserService(IUserRepository userRepository, IUserMapper userMapper, IHttpContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
            _contextAccessor = contextAccessor;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<UserDto>> GetAllAsync(CancellationToken token)
        {
            var collection = await _userRepository.GetAllAsync(token);
            return collection.Select(_userMapper.MapToDto).ToArray();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<UserDto>> GetAllFilteredAsync(Expression<Func<UserDto, bool>> expression, CancellationToken token)
        {
            var mappedExpression = _userMapper.MapToExpression(expression);
            var collection = await _userRepository.GetAllFilteredAsync(mappedExpression, token);
            return collection.Select(_userMapper.MapToDto).ToArray();
        }

        /// <inheritdoc/>
        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken token)
        {
            var entity = await _userRepository.GetByIdAsync(id, token);
            if (entity is null)
                return null;
            return _userMapper.MapToDto(entity);
        }

        /// <inheritdoc/>
        public async Task<UserDto> GetCurrentUser(CancellationToken token)
        {
            var userId = Guid.Parse(_contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return (await GetByIdAsync(userId, token))!;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Guid id, UpdateUserDto dto, CancellationToken token)
        {
            var entityExists = await _userRepository.ExistsAsync(id, token);
            if (!entityExists)
                return;

            var updatedEntity = _userMapper.MapToUser(dto);
            updatedEntity.Id = id;

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            updatedEntity.Salt = salt;
            updatedEntity.HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password, salt);

            await _userRepository.UpdateAsync(updatedEntity, token);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var entity = await _userRepository.GetByIdAsync(id, token);
            if (entity is null)
                return;
            await _userRepository.DeleteAsync(entity, token);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Guid id, CancellationToken token)
        {
            return await _userRepository.ExistsAsync(id, token);
        }
    }
}
