using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyELib.Infrastructure.DataAccess.Interfaces;

namespace MyELib.Infrastructure.DataAccess
{
    /// <inheritdoc cref="IDbContextOptionsConfigurator{TContext}"/>
    public class BaseDbContextOptionsConfigurator : IDbContextOptionsConfigurator<BaseDbContext>
    {
        private const string ConnectionString = "PostgresMyELibDb";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Инициализирует конфигуратор контекста БД.
        /// </summary>
        /// <param name="configuration">Конфигурация.</param>
        public BaseDbContextOptionsConfigurator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public void Configure(DbContextOptionsBuilder<BaseDbContext> builder)
        {
            var connectionString = _configuration.GetConnectionString(ConnectionString);
            
            builder
                .UseNpgsql(connectionString)
                .UseLazyLoadingProxies();
        }
    }
}
