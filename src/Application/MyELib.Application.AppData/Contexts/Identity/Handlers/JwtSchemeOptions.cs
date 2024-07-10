using Microsoft.AspNetCore.Authentication;

namespace MyELib.Application.AppData.Identity.Handlers
{
    /// <summary>
    /// Настройки JWT-схемы.
    /// </summary>
    public class JwtSchemeOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Активность токена.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
