using Microsoft.IdentityModel.Tokens;
using MyELib.Application.AppData.Contexts.Library.Services;
using MyELib.Application.AppData.Contexts.User.Services;
using MyELib.Contracts.Identity;

namespace MyELib.Application.AppData.Contexts.Identity.Services
{
    /// <inheritdoc cref="IAuthorizationService"/>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService _userService;
        private readonly ILibraryService _libraryService;

        /// <summary>
        /// Инициализирует сервис авторизации.
        /// </summary>
        public AuthorizationService(IUserService userService, ILibraryService libraryService)
        {
            _userService = userService;
            _libraryService = libraryService;
        }

        /// <inheritdoc/>
        public async Task<bool> HasAccessAsync(Guid id, CancellationToken token)
        {
            var currentUser = await _userService.GetCurrentUser(token);
            return currentUser is not null && (currentUser.LibraryUsers?.Any(lu => lu.UserId == currentUser.Id && lu.LibraryId == id) ?? false);
        }

        /// <inheritdoc/>
        public async Task<bool> HasAccessAsync(Guid id, AuthRolesDto role, CancellationToken token)
        {
            var currentUser = await _userService.GetCurrentUser(token);
            var result = await HasAccessAsync(id, token);
            var roleResult = currentUser.LibraryUsers?.Any(lu => HasRole(lu.Role, role));
            return result && (roleResult ?? false);
        }

        /// <inheritdoc/>
        public async Task<bool> IsUserlessAsync(Guid id, CancellationToken token)
        {
            var currentUser = await _userService.GetCurrentUser(token);
            var library = await _libraryService.GetByIdAsync(id, token);
            return currentUser is not null && library is not null && library.LibraryUsers.IsNullOrEmpty();
        }

        private static bool HasRole(AuthRolesDto userRole, AuthRolesDto requiredRole)
        {
            return userRole >= requiredRole;
        }
    }
}
