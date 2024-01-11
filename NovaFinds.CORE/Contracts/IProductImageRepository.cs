// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductImageRepository.cs" company="">
//
// </copyright>
// <summary>
//   The IProductImageRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Contracts
{
    using Domain;

    /// <summary>
    /// The IProductImageRepository interface.
    /// </summary>
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        /// <summary>
        /// The get all with product.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<ProductImage> GetAllWithProduct();

        /// <summary>
        /// The get by id with product.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<ProductImage> GetByIdWithProduct(int id);
    }
}