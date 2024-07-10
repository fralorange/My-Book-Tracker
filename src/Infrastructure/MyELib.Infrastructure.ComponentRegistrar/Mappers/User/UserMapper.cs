using MyELib.Contracts.User;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Utility;
using Riok.Mapperly.Abstractions;
using System.Linq.Expressions;
using UserEntity = MyELib.Domain.User.User;

namespace MyELib.Infrastructure.ComponentRegistrar.Mappers.User
{
    [Mapper(UseReferenceHandling = true)]
    public partial class UserMapper : IUserMapper
    {
        [MapperIgnoreSource(nameof(UserEntity.HashedPassword))]
        [MapperIgnoreSource(nameof(UserEntity.Salt))]
        public partial UserDto MapToDto(UserEntity entity);
        [MapperIgnoreTarget(nameof(UserEntity.HashedPassword))]
        [MapperIgnoreTarget(nameof(UserEntity.Salt))]
        public partial UserEntity MapToUser(UserDto dto);
        [MapperIgnoreTarget(nameof(UserEntity.HashedPassword))]
        [MapperIgnoreTarget(nameof(UserEntity.Salt))]
        [MapperIgnoreTarget(nameof(UserEntity.Id))]
        [MapperIgnoreSource(nameof(CreateUserDto.Password))]
        public partial UserEntity MapToUser(CreateUserDto dto);
        [MapperIgnoreTarget(nameof(UserEntity.HashedPassword))]
        [MapperIgnoreTarget(nameof(UserEntity.Salt))]
        [MapperIgnoreTarget(nameof(UserEntity.Id))]
        [MapperIgnoreSource(nameof(UpdateUserDto.Password))]
        public partial UserEntity MapToUser(UpdateUserDto dto);
        public Expression<Func<UserEntity, bool>> MapToExpression(Expression<Func<UserDto, bool>> expression)
            => MapperUtils.MapToExpression<UserEntity, UserDto>(expression, MapToDto);
    }
}
