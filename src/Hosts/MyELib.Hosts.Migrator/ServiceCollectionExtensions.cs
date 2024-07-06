using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyELib.Migrator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureDbConnections(configuration);

            return services;
        }

        private static IServiceCollection ConfigureDbConnections(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(GlobalConstants.ConnectionString);
            services.AddDbContext<MigrationDbContext.MigrationDbContext>(opt => opt.UseNpgsql(GlobalConstants.ConnectionString));

            return services;
        }
    }
}
