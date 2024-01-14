// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductImageDto.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ProductImage dto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DTOs
{
    using System.Text.Json.Serialization;

    /// <summary>
    ///     Product-Image dto.
    /// </summary>
    public sealed class ProductImageDto
    {
        /// <summary>
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Product-Media
        /// </remarks>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Required
        ///     Image to load
        /// </summary>
        /// <remarks>
        ///     Image in base64 to load
        /// </remarks>
        [JsonPropertyName("image")]
        public string Image { get; set; }

        /// <summary>
        ///     Description of the image
        /// </summary>
        /// <remarks>
        ///     Detailed Description of the image
        /// </remarks>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        ///     Required
        ///     External Product Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related Product
        /// </remarks>
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
    }
}