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
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Brand = product.Brand,
                Price = product.Price,
                Stock = product.Stock,
                Category = CategoryMapper.ToDomain(product.Category),
                ProductImages = ProductImageMapper.ToListDomain(product.ProductImages)
            };
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
    }
}