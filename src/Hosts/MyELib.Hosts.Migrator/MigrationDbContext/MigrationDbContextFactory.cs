using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MyELib.Migrator.MigrationDbContext
{
    /// <summary>
    /// Фабрика контекстов БД для мигратора.
    /// </summary>
    public class MigrationDbContextFactory : IDesignTimeDbContextFactory<MigrationDbContext>
    {
        /// <inheritdoc/>
        public MigrationDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var connectionString = configuration.GetConnectionString(GlobalConstants.ConnectionString);

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<MigrationDbContext>();
            dbContextOptionsBuilder.UseNpgsql(connectionString);

            return new MigrationDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
