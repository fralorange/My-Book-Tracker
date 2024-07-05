﻿using MyELib.Application.AppData;
using MyELib.Application.AppData.Contexts.Document.Repositories;
using MyELib.Application.AppData.Contexts.Document.Services;
using MyELib.Application.AppData.Contexts.Document.Validator;
using MyELib.Application.AppData.Contexts.Library.Services;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Document;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Library;
using MyELib.Infrastructure.DataAccess.Contexts.Document.Repositories;
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
            services.AddScoped<IDocumentService, DocumentService>();

            return services;
        }

        /// <summary>
        /// Добавить репозитории.
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ILibraryRepository, LibraryInMemoryRepository>();
            services.AddSingleton<IDocumentRepository, DocumentInMemoryRepository>();

            return services;
        }

        /// <summary>
        /// Добавить мапперы.
        /// </summary>
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddSingleton<ILibraryMapper, LibraryMapper>();
            services.AddSingleton<IDocumentMapper, DocumentMapper>();

            return services;
        }

        /// <summary>
        /// Добавить валидаторы.
        /// </summary>
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IDocumentValidator, DocumentValidator>();

            return services;
        }
    }
}
