// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleDto.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Role dto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DTOs
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The role.
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Role
        /// </remarks>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Required
        ///     Name of the role
        /// </summary>
        /// <remarks>
        ///     Detailed association name of the role
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}