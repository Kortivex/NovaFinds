// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductImageService.cs" company="">
//
// </copyright>
// <summary>
//   The product image service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Services
{
    using CORE.Contracts;
    using CORE.Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The product image service.
    /// </summary>
    public class ProductImageService : Service<ProductImage>, IProductImageRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductImageService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public ProductImageService(IDbContext context) : base(context) {}

        /// <summary>
        /// The get all with product.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ProductImage> GetAllWithProduct()
        {
            return this.GetAll().Include(product => product.Product);
        }

        /// <summary>
        /// The get by id with product.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ProductImage> GetByIdWithProduct(int id)
        {
            return this.GetAll().Include(product => product.Product).Where(media => media.Id == id);
        }
    }
}