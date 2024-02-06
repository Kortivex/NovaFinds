// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderProductDto.cs" company="">
//
// </copyright>
// <summary>
//   Defines the OrderProduct dto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DTOs
{
    using System.Text.Json.Serialization;

    /// <summary>
    ///     Order-Product dto
    /// </summary>
    public sealed class OrderProductDto
    {
        /// <summary>
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Order-Product
        /// </remarks>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Amount of product
        /// </summary>
        /// <remarks>
        ///     Total product requested in one order
        /// </remarks>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        ///     Amount of order
        /// </summary>
        /// <remarks>
        ///     Total amount of one product in one order
        /// </remarks>
        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        /// <summary>
        ///     External Product Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related Product
        /// </remarks>
        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }

        /// <summary>
        ///     External Order Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related Order
        /// </remarks>
        [JsonPropertyName("order_id")]
        public int OrderId { get; set; }
    }
}