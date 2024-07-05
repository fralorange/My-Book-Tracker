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
    }
}
