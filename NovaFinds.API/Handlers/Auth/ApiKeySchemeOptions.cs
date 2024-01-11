// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiKeySchemeOptions.cs" company="">
//
// </copyright>
// <summary>
//   The ApiKeyScheme Options.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.API.Handlers.Auth
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Net.Http.Headers;

    /// <summary>
    /// The ApiKeyScheme Options.
    /// </summary>
    public class ApiKeySchemeOptions : AuthenticationSchemeOptions
    {
        public const string AuthenticateScheme = "ApiKey";

        public const string ChallengeScheme = "ApiKey";

        /// <summary>
        /// Name where the API Key Header will be searched.
        /// </summary>
        /// <value></value>
        public string HeaderName { get; set; } = HeaderNames.Authorization;
    }
}