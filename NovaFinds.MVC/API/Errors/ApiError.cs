namespace NovaFinds.MVC.API.Errors
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Encapsulates an error from the NovaFinds REST API.
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Gets or sets the code for this error.
        /// </summary>
        /// <value>
        /// The code for this error.
        /// </value>
        [JsonPropertyName("code")]
        public string Code { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description for this error.
        /// </summary>
        /// <value>
        /// The description for this error.
        /// </value>
        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;
    }
}