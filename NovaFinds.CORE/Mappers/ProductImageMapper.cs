// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductImageMapper.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ProductImageMapper converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Mappers
{
    using Domain;
    using DTOs;

    public static class ProductImageMapper
    {
        /// <summary>
        ///     Convert from Domain to DTO.
        /// </summary>
        public static ProductImageDto? ToDomain(ProductImage productImage)
        {
            return new ProductImageDto
            {
                Id = productImage.Id,
                Description = productImage.Description,
                Image = productImage.Image,
                ProductId = productImage.ProductId,
            };
        }

        /// <summary>
        ///     Convert from List Domain to DTO.
        /// </summary>
        public static IEnumerable<ProductImageDto?> ToListDomain(IEnumerable<ProductImage> productImages)
        {
            var productImagesDto = new List<ProductImageDto?>();
            foreach (var productImage in productImages){ productImagesDto.Add(ToDomain(productImage)); }

            return productImagesDto;
        }
    }
}