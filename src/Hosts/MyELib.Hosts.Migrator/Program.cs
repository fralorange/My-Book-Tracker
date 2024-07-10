using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyELib.Migrator;
using MyELib.Migrator.MigrationDbContext;

namespace MyELib.Hosts.Migrator
{
#pragma warning disable CS1591
    public class Program
    {
        public static async Task Main(string[] args)
#pragma warning restore CS1591
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddServices(hostContext.Configuration);
                })
                .Build();

            await MigrateDatabaseAsync(host.Services);
            await host.RunAsync();
        }

        private static async Task MigrateDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MigrationDbContext>();
            await context.Database.MigrateAsync();
        }
    }
}