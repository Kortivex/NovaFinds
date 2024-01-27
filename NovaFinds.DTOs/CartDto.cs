// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartDto.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Cart dto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DTOs
{
    using System.Text.Json.Serialization;

    /// <summary>
    ///     Cart dto
    /// </summary>
    public sealed class CartDto
    {
        /// <summary>
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Shopping Cart
        /// </remarks>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Username
        /// </summary>
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        
        /// <summary>
        ///     External User Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related User
        /// </remarks>
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
}