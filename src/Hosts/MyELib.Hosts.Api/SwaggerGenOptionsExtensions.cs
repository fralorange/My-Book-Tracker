using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace MyELib.Hosts.Api
{
    /// <summary>
    /// Класс расширенияй для <see cref="SwaggerGenOptions"/>
    /// </summary>
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Добавление документации сваггера.
        /// </summary>
        /// <param name="options">Параметры сваггера.</param>
        public static SwaggerGenOptions AddSwaggerDoc(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "My E-Lib", Version = "v1" });

            return options;
        }

        /// <summary>
        /// Добавление XML-документации в сваггер.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SwaggerGenOptions AddSwaggerXML(this SwaggerGenOptions options)
        {
            foreach (var filePath in Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "*.xml"))
            {
                options.IncludeXmlComments(filePath);
            }

            return options;
        }

        /// <summary>
        /// Добавление <see cref="OpenApiSecurityScheme"/> и <see cref="OpenApiSecurityRequirement"/> в Swagger для реализации авторизации.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SwaggerGenOptions AddSwaggerSecurity(this SwaggerGenOptions options)
        {
            #region SecurityDefinition
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                                    Enter 'Bearer' [space] and then your token in the text input below.
                                    Example: 'Bearer secretKey'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
            });
            #endregion
            #region SecurityRequirement
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
            #endregion

            return options;
        }
    }
}
