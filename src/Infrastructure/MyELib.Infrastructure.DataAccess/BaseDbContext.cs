using Microsoft.EntityFrameworkCore;
using MyELib.Infrastructure.DataAccess.Contexts.Document.Configuration;
using MyELib.Infrastructure.DataAccess.Contexts.Library.Configuration;

namespace MyELib.Infrastructure.DataAccess
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    public class BaseDbContext : DbContext
    {
        /// <inheritdoc/>
        public BaseDbContext(DbContextOptions options) : base(options) { }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LibraryConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
