// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductMapper.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ProductMapper converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Mappers
{
    using Domain;
    using DTOs;

    public static class ProductMapper
    {
        /// <summary>
        ///     Convert from Domain to DTO.
        /// </summary>
        public static ProductDto? ToDomain(Product product)
        {
            var newProduct = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Brand = product.Brand,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                Category = null,
                ProductImages = null
            };

            if (product.Category != null && product.Category.Id != 0){ newProduct.Category = CategoryMapper.ToDomain(product.Category); }
            if (product.ProductImages != null  && product.ProductImages.Any()){ newProduct.ProductImages = ProductImageMapper.ToListDomain(product.ProductImages); }

            return newProduct;
        }

        /// <summary>
        ///     Convert from List Domain to DTO.
        /// </summary>
        public static IEnumerable<ProductDto?> ToListDomain(IEnumerable<Product> products)
        {
            var productsDto = new List<ProductDto?>();
            foreach (var product in products){ productsDto.Add(ToDomain(product)); }

            return productsDto;
        }

        /// <summary>
        ///     Convert from Domain to DB.
        /// </summary>
        public static Product? ToDb(ProductDto product)
        {
            return new Product
            {
                Name = product.Name,
                Description = product.Description,
                Brand = product.Brand,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                Category = null,
                ProductImages = null
            };
        }

        /// <summary>
        ///     Convert from List Domain to DB.
        /// </summary>
        public static IEnumerable<Product?> ToListDb(IEnumerable<ProductDto> products)
        {
            var productsDb = new List<Product?>();
            foreach (var product in products){ productsDb.Add(ToDb(product)); }

            return productsDb;
        }
    }
}