using Microsoft.AspNetCore.Http;
using MyELib.Application.AppData.Contexts.LibraryUser.Repositories;
using MyELib.Contracts.LibraryUser;
using MyELib.Domain.Identity;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.LibraryUser;
using System.Security.Claims;

namespace MyELib.Application.AppData.Contexts.LibraryUser.Services
{
    /// <inheritdoc cref="ILibraryUserService"/>
    public class LibraryUserService : ILibraryUserService
    {
        private readonly ILibraryUserRepository _libraryUserRepository;
        private readonly ILibraryUserMapper _libraryUserMapper;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Инициализирует сервис библиотек-пользователей.
        /// </summary>
        /// <param name="libraryUserRepository"></param>
        /// <param name="libraryUserMapper"></param>
        /// <param name="contextAccessor"></param>
        public LibraryUserService(
            ILibraryUserRepository libraryUserRepository,
            ILibraryUserMapper libraryUserMapper,
            IHttpContextAccessor contextAccessor)
        {
            _libraryUserRepository = libraryUserRepository;
            _libraryUserMapper = libraryUserMapper;
            _contextAccessor = contextAccessor;
        }

        /// <inheritdoc/>
        public async Task<LibraryUserDto?> GetByIdAsync(Guid id, CancellationToken token)
        {
            var entity = await _libraryUserRepository.GetByIdAsync(id, token);
            if (entity is null)
                return null;
            return _libraryUserMapper.MapToDto(entity);
        }

        /// <inheritdoc/>
        public async Task<Guid> CreateAsync(CreateCurrentLibraryUserDto dto, CancellationToken token)
        {
            var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Guid.Empty;

            var userIdGuid = Guid.Parse(userId);

            var entity = _libraryUserMapper.MapToLibraryUser(dto);
            entity.Role = AuthRoles.Owner;
            entity.UserId = userIdGuid;

            return await _libraryUserRepository.CreateAsync(entity, token);
        }

        /// <inheritdoc/>
        public async Task<Guid> CreateAsync(CreateLibraryUserDto dto, CancellationToken token)
        {
            var entity = _libraryUserMapper.MapToLibraryUser(dto);
            return await _libraryUserRepository.CreateAsync(entity, token);
        }

        /// <inheritdoc/>
        public async Task PatchAsync(Guid id, UpdateLibraryUserDto dto, CancellationToken token)
        {
            var entity = await _libraryUserRepository.GetByIdAsync(id, token);
            if (entity is null)
                return;

            entity.Role = (AuthRoles)dto.Role;

            await _libraryUserRepository.UpdateAsync(entity, token);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var entity = await _libraryUserRepository.GetByIdAsync(id, token);
            if (entity is null)
                return;

            await _libraryUserRepository.DeleteAsync(entity, token);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Guid id, CancellationToken token)
        {
            return await _libraryUserRepository.ExistsAsync(id, token);
        }
    }
}
