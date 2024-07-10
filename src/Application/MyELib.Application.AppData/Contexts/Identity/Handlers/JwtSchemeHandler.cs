using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace MyELib.Application.AppData.Identity.Handlers
{
    /// <summary>
    /// Обработчик схемы JWT.
    /// </summary>
    public class JwtSchemeHandler : AuthenticationHandler<JwtSchemeOptions>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Инициализирует обработчик.
        /// </summary>
        public JwtSchemeHandler(
            IOptionsMonitor<JwtSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IHttpContextAccessor contextAccessor,
            IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            bool tokenPresented = _contextAccessor.HttpContext
                .Request.Cookies.TryGetValue("token", out var token);

            if (!tokenPresented)
            {
                return Task.FromResult(AuthenticateResult.Fail("Fail. Token Not Found!"));
            }

            var handler = new JwtSecurityTokenHandler();

            var parts = token!.Split(".".ToCharArray());

            var header = parts[0];
            var payload = parts[1];
            var signature = parts[2];

            var bytesToSign = Encoding.UTF8.GetBytes($"{header}.{payload}");
            var secret = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var alg = new HMACSHA256(secret);
            var hash = alg.ComputeHash(bytesToSign);

            var calculatedSignature = Base64UrlEncode(hash);

            if (!calculatedSignature.Equals(signature))
            {
                return Task.FromResult(AuthenticateResult.Fail("Fail. Invalid Token!"));
            }

            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims;

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private static string Base64UrlEncode(byte[] bytes)
        {
            var value = Convert.ToBase64String(bytes);

            value = value.Split('=')[0];
            value = value.Replace('+', '-');
            value = value.Replace('/', '_');

            return value;
        }
    }
}
