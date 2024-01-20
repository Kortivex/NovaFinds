// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryExt.cs" company="">
//
// </copyright>
// <summary>
//   Defines the QueryExt type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.API.Filters
{

    /// <summary>
    /// The generic queryable extensions.
    /// </summary>
    public static class QueryExt
    {
        /// <summary>
        /// The get paged for the IQueryable objects.
        /// </summary>
        /// <param name="queryable">
        /// The queryable.
        /// </param>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <typeparam name="TEntity">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public static IQueryable<TEntity> GetPaged<TEntity>(this IQueryable<TEntity> queryable, int page, int size)
            where TEntity : class
        {
            if (page == 0)
            {
                page = 1;
            }

            return queryable.Skip((page - 1) * size).Take(size);
        }
    }
}