// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="">
//
// </copyright>
// <summary>
//   Defines the IRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Contracts
{
    using System.Linq.Expressions;

    /// <summary>
    /// Generic Repository Interface.
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Data Base Context.
        /// </summary>
        IDbContext Context { get; }

        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task CreateAsync(TEntity entity);

        /// <summary>
        /// The create range async.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task CreateRangeAsync(ICollection<TEntity> entities);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task Delete(TEntity entity);

        /// <summary>
        /// The delete by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task DeleteByIdAsync(long id);

        /// <summary>
        /// The delete range.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        void DeleteRange(ICollection<TEntity> entities);

        /// <summary>
        /// The dispose async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task DisposeAsync();

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// The get by element async.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<TEntity> GetByElementAsync(TEntity entity);

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<TEntity?> GetByIdAsync(long id);

        /// <summary>
        /// The save changes async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void Update(TEntity entity);

        /// <summary>
        /// The update range.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        void UpdateRange(ICollection<TEntity> entities);
    }
}