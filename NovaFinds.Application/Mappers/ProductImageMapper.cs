// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductImageMapper.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ProductImageMapper converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Mappers
{
    using CORE.Domain;
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

        /// <summary>
        ///     Convert from Domain to DB.
        /// </summary>
        public static ProductImage? ToDb(ProductImageDto productImage)
        {
            return new ProductImage
            {
                Description = productImage.Description,
                ProductId = productImage.ProductId,
                Image = productImage.Image,
                Product = null
            };
        }

        /// <summary>
        ///     Convert from List Domain to DB.
        /// </summary>
        public static IEnumerable<ProductImage?> ToListDb(IEnumerable<ProductImageDto> productImages)
        {
            var productImagesDb = new List<ProductImage?>();
            foreach (var productImage in productImages){ productImagesDb.Add(ToDb(productImage)); }

            return productImagesDb;
        }
    }
}