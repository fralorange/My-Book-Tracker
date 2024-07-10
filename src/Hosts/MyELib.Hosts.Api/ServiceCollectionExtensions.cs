using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using MyELib.Application.AppData;
using MyELib.Application.AppData.Contexts.Document.Repositories;
using MyELib.Application.AppData.Contexts.Document.Services;
using MyELib.Application.AppData.Contexts.Document.Validator;
using MyELib.Application.AppData.Contexts.Identity.Services;
using MyELib.Application.AppData.Contexts.Library.Services;
using MyELib.Application.AppData.Contexts.LibraryUser.Repositories;
using MyELib.Application.AppData.Contexts.LibraryUser.Services;
using MyELib.Application.AppData.Contexts.User.Repositories;
using MyELib.Application.AppData.Contexts.User.Services;
using MyELib.Application.AppData.Identity.Handlers;
using MyELib.Application.AppData.Identity.Services;
using MyELib.Domain.Identity;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Document;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.Library;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.LibraryUser;
using MyELib.Infrastructure.ComponentRegistrar.Mappers.User;
using MyELib.Infrastructure.DataAccess;
using MyELib.Infrastructure.DataAccess.Contexts.Document.Repositories;
using MyELib.Infrastructure.DataAccess.Contexts.Library.Repositories;
using MyELib.Infrastructure.DataAccess.Contexts.LibraryUser.Repositories;
using MyELib.Infrastructure.DataAccess.Contexts.User.Repositories;
using MyELib.Infrastructure.DataAccess.Interfaces;
using MyELib.Infrastructure.Repository;
using System.Text;

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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILibraryUserService, LibraryUserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();

            return services;
        }

        /// <summary>
        /// Добавить репозитории.
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILibraryUserRepository, LibraryUserRepository>();

            return services;
        }

        /// <summary>
        /// Добавить мапперы.
        /// </summary>
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddSingleton<ILibraryMapper, LibraryMapper>();
            services.AddSingleton<IDocumentMapper, DocumentMapper>();
            services.AddSingleton<IUserMapper, UserMapper>();
            services.AddSingleton<ILibraryUserMapper, LibraryUserMapper>();

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

        /// <summary>
        /// Добавить контекст БД.
        /// </summary>
        public static IServiceCollection AddDbContextConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IDbContextOptionsConfigurator<BaseDbContext>, BaseDbContextOptionsConfigurator>();
            services.AddDbContext<BaseDbContext>((serviceProvider, options) =>
            {
                var configurator = serviceProvider.GetRequiredService<IDbContextOptionsConfigurator<BaseDbContext>>();
                configurator.Configure((DbContextOptionsBuilder<BaseDbContext>)options);
            });
            services.AddScoped<DbContext, BaseDbContext>();

            return services;
        }

        /// <summary>
        /// Добавить аутентификацию и авторизацию.
        /// </summary>
        public static IServiceCollection AddAuth(this IServiceCollection services, IHostApplicationBuilder builder)
        {
            #region Authentication
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    };
                })
                .AddScheme<JwtSchemeOptions, JwtSchemeHandler>(AuthSchemes.JWT, options => { });
            #endregion
            #region Authorization
            services.AddAuthorization();
            #endregion
            return services;
        }
    }
}
