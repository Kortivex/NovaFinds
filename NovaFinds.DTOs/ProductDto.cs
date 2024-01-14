// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductDto.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Product dto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DTOs
{
    using System.Text.Json.Serialization;

    /// <summary>
    ///     Product table
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        ///     Identity, Indexed, Required
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Product
        /// </remarks>
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        /// <summary>
        ///     Required
        ///     Name of the product
        /// </summary>
        /// <remarks>
        ///     Detailed association name of the product
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        /// <summary>
        ///     Full Description of the product
        /// </summary>
        /// <remarks>
        ///     Detailed Description of the product
        /// </remarks>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        /// <summary>
        ///     Required
        ///     Brand of the product
        /// </summary>
        /// <remarks>
        ///     Detailed association brand of the product
        /// </remarks>
        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        /// <summary>
        ///     Required, Default value = 0
        ///     Price of the product
        /// </summary>
        /// <remarks>
        ///     Value of the product
        /// </remarks>
        [JsonPropertyName("price")]
        public double Price { get; set; }

        /// <summary>
        ///     Required, Default value = 0
        ///     Units Available in warehouse
        /// </summary>
        /// <remarks>
        ///     Units in the warehouse
        /// </remarks>
        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [JsonPropertyName("category")]
        public CategoryDto? Category { get; set;}
        
        [JsonPropertyName("productImages")]
        public IEnumerable<ProductImageDto?>? ProductImages { get; set; }
        
    }
}