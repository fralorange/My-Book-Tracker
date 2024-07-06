using Microsoft.EntityFrameworkCore;

namespace MyELib.Infrastructure.DataAccess.Interfaces
{
    /// <summary>
    /// Конфигуратор контекста БД.
    /// </summary>
    /// <typeparam name="TContext">Контекст БД.</typeparam>
    public interface IDbContextOptionsConfigurator<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Выполняет конфигурацию контекста.
        /// </summary>
        /// <param name="builder">Билдер параметров.</param>
        void Configure(DbContextOptionsBuilder<TContext> builder);
    }
}
