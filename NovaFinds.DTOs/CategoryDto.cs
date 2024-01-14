// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryDto.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Category dto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DTOs
{
    using System.Text.Json.Serialization;

    /// <summary>
    ///     Category dto
    /// </summary>
    public sealed class CategoryDto
    {
        /// <summary>
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Category
        /// </remarks>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Name of the category
        /// </summary>
        /// <remarks>
        ///     Detailed association name of the category
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}