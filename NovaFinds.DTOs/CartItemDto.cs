// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartItemDto.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Cart-Item dto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DTOs
{
    using System.Text.Json.Serialization;

    /// <summary>
    ///     Cart-Item dto
    /// </summary>
    public sealed class CartItemDto
    {
        /// <summary>
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of CartItem
        /// </remarks>
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        /// <summary>
        ///     Amount of product
        /// </summary>
        /// <remarks>
        ///     Total product requested
        /// </remarks>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        ///     External Cart Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related Cart
        /// </remarks>
        [JsonPropertyName("cart_id")]
        public int CartId { get; set; }

        /// <summary>
        ///     External Product Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related Product
        /// </remarks>
        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }
    }
}