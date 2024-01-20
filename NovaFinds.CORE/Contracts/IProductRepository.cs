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
        /// The find by name size.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<Product> FindByNameSize(string name, int size);
        
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

        /// <summary>
        /// The get with category image size.
        /// </summary>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<Product> GetWithCategoryImageSize(int size);
        
        /// <summary>
        /// The sort by media order.
        /// </summary>
        /// <param name="products">
        /// The products.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        ICollection<Product> SortByImageOrder(ICollection<Product> products);
    }
}