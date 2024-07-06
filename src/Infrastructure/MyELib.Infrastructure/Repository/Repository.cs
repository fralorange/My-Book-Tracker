using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MyELib.Infrastructure.Repository
{
    /// <inheritdoc cref="IRepository{TEntity}"/>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Контекст БД.
        /// </summary>
        protected DbContext DbContext { get; }
        /// <summary>
        /// Множество БД.
        /// </summary>
        protected DbSet<TEntity> DbSet { get; }

        /// <summary>
        /// Инициализирует базовые репозиторий.
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetAllFiltered(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await DbSet.FindAsync(id, token);
        }

        /// <inheritdoc/>
        public async Task AddAsync(TEntity entity, CancellationToken token)
        {
            await DbSet.AddAsync(entity, token);
            await DbContext.SaveChangesAsync(token);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(TEntity entity, CancellationToken token)
        {
            DbSet.Update(entity);
            await DbContext.SaveChangesAsync(token);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(TEntity entity, CancellationToken token)
        {
            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync(token);
        }
    }
}
