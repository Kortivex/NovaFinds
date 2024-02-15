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
    using System.ComponentModel;
    using System.Text.Json.Serialization;

    /// <summary>
    ///    The Product.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
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

        /// <summary>
        ///     External Product-Category Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related category
        /// </remarks>
        [JsonPropertyName("category_id")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [JsonPropertyName("category")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CategoryDto? Category { get; set; }

        [JsonPropertyName("product_images")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<ProductImageDto?>? ProductImages { get; set; }
    }
}