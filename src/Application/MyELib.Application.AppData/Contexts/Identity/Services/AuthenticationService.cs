using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyELib.Application.AppData.Contexts.User.Repositories;
using MyELib.Application.AppData.Identity.Services;
using MyELib.Contracts.User;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyELib.Application.AppData.Contexts.Identity.Services
{
    /// <inheritdoc cref="IAuthenticationService"/>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserMapper _userMapper;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Инициализирует сервис аутентификации и регистрации.
        /// </summary>
        public AuthenticationService(IUserRepository userRepository, IUserMapper userMapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<string> LoginAsync(LoginUserDto dto, CancellationToken token)
        {
            var user = await _userRepository.GetByPredicateAsync(usr => usr.Username == dto.Username, token);
            if (user is null)
                return string.Empty;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password, user.Salt);
            if (hashedPassword != user.HashedPassword)
                return string.Empty;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken
            (
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: signIn
            );

            var result = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return result;
        }

        /// <inheritdoc/>
        public async Task<Guid> RegisterAsync(CreateUserDto dto, CancellationToken token)
        {
            var testUser = await _userRepository.GetByPredicateAsync(usr => usr.Username == dto.Username, token);
            if (testUser is not null)
                return Guid.Empty;

            var user = _userMapper.MapToUser(dto);
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            user.Salt = salt;
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password, salt);

            return await _userRepository.CreateAsync(user, token);
        }
    }
}
