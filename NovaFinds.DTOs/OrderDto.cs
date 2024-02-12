// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderDto.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Order dto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DTOs
{
    using System.ComponentModel;
    using System.Text.Json.Serialization;

    /// <summary>
    ///     Order dto
    /// </summary>
    public sealed class OrderDto
    {
        /// <summary>
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Category
        /// </remarks>
        [JsonPropertyName("id")]
        [DisplayName("Order Id")]
        public int Id { get; set; }

        /// <summary>
        ///     Date
        /// </summary>
        /// <remarks>
        ///     Date when the Order was created
        /// </remarks>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        ///     Status of the Order
        /// </summary>
        /// <remarks>
        ///     Status of the Order by OrderStatusType
        /// </remarks>
        [JsonPropertyName("status")]
        public OrderStatusType Status { get; set; }

        /// <summary>
        ///     Id of the User
        /// </summary>
        /// <remarks>
        ///     Id assigned to User
        /// </remarks>
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }

    public enum OrderStatusType
    {
        /// <summary>
        /// The pending.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The paid.
        /// </summary>
        Paid = 1,

        /// <summary>
        /// The processing.
        /// </summary>
        Processing = 2,

        /// <summary>
        /// The shipped.
        /// </summary>
        Shipped = 3,

        /// <summary>
        /// The delivered.
        /// </summary>
        Delivered = 4
    }
}