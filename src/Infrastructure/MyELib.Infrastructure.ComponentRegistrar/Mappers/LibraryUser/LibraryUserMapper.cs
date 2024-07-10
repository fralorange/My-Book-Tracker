using MyELib.Contracts.LibraryUser;
using Riok.Mapperly.Abstractions;
using LibraryUserEntity = MyELib.Domain.LibraryUser.LibraryUser;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.LibraryUser
{
    [Mapper(UseReferenceHandling = true)]
    public partial class LibraryUserMapper : ILibraryUserMapper
    {
        [MapperIgnoreSource(nameof(LibraryUserEntity.Library))]
        [MapperIgnoreSource(nameof(LibraryUserEntity.User))]
        public partial LibraryUserDto MapToDto(LibraryUserEntity entity);
        [MapperIgnoreTarget(nameof(LibraryUserEntity.Library))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.User))]
        public partial LibraryUserEntity MapToLibraryUser(LibraryUserDto dto);
        [MapperIgnoreTarget(nameof(LibraryUserEntity.Library))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.User))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.Id))]
        public partial LibraryUserEntity MapToLibraryUser(CreateLibraryUserDto dto);
        [MapperIgnoreTarget(nameof(LibraryUserEntity.Library))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.User))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.UserId))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.Role))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.Id))]
        public partial LibraryUserEntity MapToLibraryUser(CreateCurrentLibraryUserDto dto);
        [MapperIgnoreTarget(nameof(LibraryUserEntity.Library))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.User))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.LibraryId))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.UserId))]
        [MapperIgnoreTarget(nameof(LibraryUserEntity.Id))]
        public partial LibraryUserEntity MapToLibraryUser(UpdateLibraryUserDto dto);
    }
}
