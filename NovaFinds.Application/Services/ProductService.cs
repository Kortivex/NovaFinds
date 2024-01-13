// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductService.cs" company="">
//
// </copyright>
// <summary>
//   The product service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Services
{
    using CORE.Contracts;
    using CORE.Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The product service.
    /// </summary>
    public class ProductService : Service<Product>, IProductRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public ProductService(IDbContext context) : base(context) {}

        /// <summary>
        /// The get by category id with media and brand.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Product> GetByCategoryIdWithImages(int id)
        {
            return IncludeMedia(this.Find(product => product.CategoryId == id));
        }

        /// <summary>
        /// The get with category image size.
        /// </summary>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Product> GetWithCategoryImageSize(int size)
        {
            return IncludeCategoryImages(GetAll()).Take(size);
        }

        /// <summary>
        /// The sort by image order.
        /// </summary>
        /// <param name="products">
        /// The products.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection"/>.
        /// </returns>
        public ICollection<Product> SortByImageOrder(ICollection<Product> products)
        {
            if (products.Count == 0) return products;
            foreach (var product in products)
                if (product.ProductImages.Any())
                    product.ProductImages = product.ProductImages.OrderBy(c => c.Id).ToList();

            return products;
        }

        /// <summary>
        /// The include media brand.
        /// </summary>
        /// <param name="products">
        /// The products.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        private static IQueryable<Product> IncludeMedia(IQueryable<Product> products)
        {
            return products
                .Include(product => product.ProductImages)
                .Where(product => product.ProductImages.Count() != 0);
        }

        /// <summary>
        /// The include category images.
        /// </summary>
        /// <param name="products">
        /// The products.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        private static IQueryable<Product> IncludeCategoryImages(IQueryable<Product> products)
        {
            return products.Include(product => product.Category)
                .Include(product => product.ProductImages)
                .Where(product => product.ProductImages.Count() != 0);
        }
    }
}