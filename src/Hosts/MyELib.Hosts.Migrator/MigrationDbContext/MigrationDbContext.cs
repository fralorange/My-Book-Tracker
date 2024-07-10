using Microsoft.EntityFrameworkCore;
using MyELib.Infrastructure.DataAccess;

namespace MyELib.Migrator.MigrationDbContext
{
    /// <summary>
    /// Контекст БД для мигратора.
    /// </summary>
    public class MigrationDbContext : BaseDbContext
    {
        public MigrationDbContext(DbContextOptions options) : base(options) { }
    }
}
