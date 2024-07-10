using System.Linq.Expressions;

namespace MyELib.Infrastructure.Repository
{
    /// <summary>
    /// Базовый репозиторий.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Возвращает коллекцию.
        /// </summary>
        /// <returns>Коллекцию <see href="TEntity"/>.</returns>
        IQueryable<TEntity> GetAll();
        /// <summary>
        /// Возвращает отфильтрованную коллекцию.
        /// </summary>
        /// <param name="predicate">Параметр фильтрации.</param>
        /// <returns>Отфильтрованную коллекцию <see href="TEntity"/>.</returns>
        IQueryable<TEntity> GetAllFiltered(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Возвращает сущность типа <see href="TEntity"/> по заданному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Сущность <see href="TEntity"/></returns>
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token);
        /// <summary>
        /// Возвращает сущность типа <see href="TEntity"/> по параметру (предикате).
        /// </summary>
        /// <param name="predicate">Параметр поиска.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Сущность <see href="TEntity"/></returns>
        Task<TEntity?> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token);
        /// <summary>
        /// Добавляет сущность.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task AddAsync(TEntity entity, CancellationToken token);
        /// <summary>
        /// Обновляет атрибуты существующей сущности новыми.
        /// </summary>
        /// <param name="entity">Сущность с новыми характеристиками.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task UpdateAsync(TEntity entity, CancellationToken token);
        /// <summary>
        /// Удаляет сущность.
        /// </summary>
        /// <param name="entity">Сущность.</param>
        /// <param name="token">Токен отмены операции.</param>
        Task DeleteAsync(TEntity entity, CancellationToken token);
        /// <summary>
        /// Проверяет на сущность.
        /// </summary>
        /// <param name="id">Уникальный идентфикатор.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid id, CancellationToken token);
    }
}
