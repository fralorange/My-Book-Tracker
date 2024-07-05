using MyELib.Application.AppData;
using MyELib.Application.AppData.Contexts.Library.Services;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Library;
using MyELib.Infrastructure.DataAccess.Contexts.Library.Repositories;

namespace MyELib.Hosts.Api
{
    /// <summary>
    /// Класс расширений для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавить сервисы.
        /// </summary>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ILibraryService, LibraryService>();
            return services;
        }

        /// <summary>
        /// Добавить репозитории.
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ILibraryRepository, LibraryInMemoryRepository>();
            return services;
        }

        /// <summary>
        /// Добавить мапперы.
        /// </summary>
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddSingleton<ILibraryMapper, LibraryMapper>();
            return services;
        }
    }
}
