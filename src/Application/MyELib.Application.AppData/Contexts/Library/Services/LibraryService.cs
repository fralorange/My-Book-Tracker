using MyELib.Application.AppData.Contexts.Library.Services;
using MyELib.Contracts.Library;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Library;
using System.Linq.Expressions;

namespace MyELib.Application.AppData
{
    /// <inheritdoc cref="ILibraryService"/>
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly ILibraryMapper _libraryMapper;

        /// <summary>
        /// Инициализирует сервис библиотек.
        /// </summary>
        public LibraryService(ILibraryRepository libraryRepository, ILibraryMapper libraryMapper)
        {
            _libraryRepository = libraryRepository;
            _libraryMapper = libraryMapper;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<LibraryDto>> GetAllAsync(CancellationToken token)
        {
            var collection = await _libraryRepository.GetAllAsync(token);
            return collection.Select(_libraryMapper.MapToDto).ToArray();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<LibraryDto>> GetAllFilteredAsync(Expression<Func<LibraryDto, bool>> expression, CancellationToken token)
        {
            var entityExpression = _libraryMapper.MapToExpression(expression);
            var collection = await _libraryRepository.GetAllFilteredAsync(entityExpression, token);
            return collection.Select(_libraryMapper.MapToDto).ToArray();
        }

        /// <inheritdoc/>
        public async Task<LibraryDto?> GetByIdAsync(Guid id, CancellationToken token)
        {
            var entity = await _libraryRepository.GetByIdAsync(id, token);
            if (entity is null)
                return null;
            return _libraryMapper.MapToDto(entity);
        }

        /// <inheritdoc/>
        public async Task<Guid> CreateAsync(CreateLibraryDto dto, CancellationToken token)
        {
            if (dto is null)
                return Guid.Empty;
            var entity = _libraryMapper.MapToLibrary(dto);
            var createdId = await _libraryRepository.CreateAsync(entity, token);

            return createdId;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(UpdateLibraryDto dto, Guid id, CancellationToken token)
        {
            var exists = await ExistsAsync(id, token);
            if (!exists)
                return;
            var updatedEntity = _libraryMapper.MapToLibrary(dto);
            updatedEntity.Id = id;
            await _libraryRepository.UpdateAsync(updatedEntity, token);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var entity = await _libraryRepository.GetByIdAsync(id, token);
            if (entity is null)
                return;
            await _libraryRepository.DeleteAsync(entity, token);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Guid id, CancellationToken token)
        {
            return await _libraryRepository.ExistsAsync(id, token);
        }
    }
}