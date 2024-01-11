// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductRepository.cs" company="">
//
// </copyright>
// <summary>
//   Defines the IProductRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Contracts
{
    using Domain;

    /// <summary>
    /// The ProductRepository interface.
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// The get by category id and with image.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<Product> GetByCategoryIdWithImages(int id);
    }
}