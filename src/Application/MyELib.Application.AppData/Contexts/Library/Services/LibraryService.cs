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

        public LibraryService(ILibraryRepository libraryRepository, ILibraryMapper libraryMapper)
        {
            _libraryRepository = libraryRepository;
            _libraryMapper = libraryMapper;
        }

        public async Task<IReadOnlyCollection<LibraryDto>> GetAllAsync(CancellationToken token)
        {
            var collection = await _libraryRepository.GetAllAsync(token);
            return collection.Select(_libraryMapper.MapToDto).ToArray();
        }

        public async Task<IReadOnlyCollection<LibraryDto>> GetAllFilteredAsync(Expression<Func<LibraryDto, bool>> expression, CancellationToken token)
        {
            var entityExpression = _libraryMapper.MapToExpression(expression);
            var collection = await _libraryRepository.GetAllFilteredAsync(entityExpression, token);
            return collection.Select(_libraryMapper.MapToDto).ToArray();
        }

        public async Task<LibraryDto?> GetByIdAsync(Guid id, CancellationToken token)
        {
            var entity = await _libraryRepository.GetByIdAsync(id, token);
            if (entity is null)
                return null;
            return _libraryMapper.MapToDto(entity);
        }

        public async Task<Guid> CreateAsync(CreateLibraryDto dto, CancellationToken token)
        {
            if (dto is null)
                return Guid.Empty;
            var entity = _libraryMapper.MapToLibrary(dto);
            return await _libraryRepository.CreateAsync(entity, token);
        }
        public async Task UpdateAsync(UpdateLibraryDto dto, Guid id, CancellationToken token)
        {
            var targetDto = await GetByIdAsync(id, token);
            if (targetDto is null)
                return;
            var updatedEntity = _libraryMapper.MapToLibrary(dto);
            updatedEntity.Id = targetDto.Id;
            await _libraryRepository.UpdateAsync(updatedEntity, token);
        }

        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var targetDto = await GetByIdAsync(id, token);
            if (targetDto is null)
                return;
            var entity = _libraryMapper.MapToLibrary(targetDto);
            await _libraryRepository.DeleteAsync(entity, token);
        }
    }
}