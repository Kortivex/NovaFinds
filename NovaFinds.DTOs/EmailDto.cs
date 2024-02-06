// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailDto.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Email dto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DTOs
{
    using System.Text.Json.Serialization;

    /// <summary>
    ///     Email dto
    /// </summary>
    public sealed class EmailDto
    {
        /// <summary>
        ///     Email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        ///     Email Subject
        /// </summary>
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        /// <summary>
        ///     Email Message
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        ///     Email From
        /// </summary>
        [JsonPropertyName("from")]
        public string From { get; set; }

        /// <summary>
        ///     Email Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}