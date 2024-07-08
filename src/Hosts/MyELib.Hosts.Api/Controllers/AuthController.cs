using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyELib.Application.AppData.Identity.Services;
using MyELib.Contracts.User;

namespace MyELib.Hosts.Api.Controllers
{
    /// <summary>
    /// Контроллер аутентификации и регистрации.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        /// <summary>
        /// Инициализирует котроллер аутентификации и регистрации.
        /// </summary>
        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Логин в систему.
        /// </summary>
        /// <param name="model">Модель логина.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromForm] LoginUserDto model, CancellationToken token)
        {
            var securityToken = await _authService.LoginAsync(model, token);
            if (string.IsNullOrEmpty(securityToken))
                return BadRequest();
            return Ok(securityToken);
        }

        /// <summary>
        /// Регистрация в системе.
        /// </summary>
        /// <param name="model">Регистрируемая модель.</param>
        /// <param name="token">Токен отмены операции.</param>
        [HttpPost("register")]
        [ActionName(nameof(PostAsync))]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromForm] CreateUserDto model, CancellationToken token) 
        {
            var id = await _authService.RegisterAsync(model, token);
            if (id == Guid.Empty)
                return BadRequest();
            return CreatedAtAction(nameof(PostAsync), id);
        }
    }
}
