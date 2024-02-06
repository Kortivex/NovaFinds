// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Service.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Service type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Services
{
    using CORE.Contracts;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;

    /// <summary>
    /// The generic service, for all type that implement IEntity.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public class Service<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// The db context.
        /// </summary>
        public IDbContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Service{TEntity}"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        protected Service(IDbContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task CreateAsync(TEntity entity)
        {
            await this.Context.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// The create range async.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task CreateRangeAsync(ICollection<TEntity> entities)
        {
            await this.Context.Set<TEntity>().AddRangeAsync(entities).ConfigureAwait(false);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task Delete(TEntity entity)
        {
            var exist = await this.GetByElementAsync(entity).ConfigureAwait(false);
            this.Context.Set<TEntity>().Remove(exist);
        }

        /// <summary>
        /// The delete by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task DeleteByIdAsync(long id)
        {
            var entity = await this.GetByIdAsync(id).ConfigureAwait(false);
            if (entity != null) this.Context.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// The delete range.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        public void DeleteRange(ICollection<TEntity> entities)
        {
            this.Context.Set<TEntity>().RemoveRange(entities);
        }

        /// <summary>
        /// The dispose async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task DisposeAsync()
        {
            await this.Context.DisposeAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Context.Set<TEntity>().Where(predicate);
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TEntity> GetAll()
        {
            return this.Context.Set<TEntity>().AsNoTracking();
        }

        /// <summary>
        /// The get by element async.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<TEntity> GetByElementAsync(TEntity entity)
        {
            return await this.Context.Set<TEntity>().FindAsync(entity.Id) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<TEntity?> GetByIdAsync(long id)
        {
            return await this.Context.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// The save changes async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<int> SaveChangesAsync()
        {
            return await this.Context.SaveChangesAsync();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public void Update(TEntity entity)
        {
            this.Context.Set<TEntity>().Update(entity);
        }

        /// <summary>
        /// The update range.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        public void UpdateRange(ICollection<TEntity> entities)
        {
            this.Context.Set<TEntity>().UpdateRange(entities);
        }
    }
}