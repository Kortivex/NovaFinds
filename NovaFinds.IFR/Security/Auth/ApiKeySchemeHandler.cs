// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiKeySchemeHandler.cs" company="">
//
// </copyright>
// <summary>
//   The ApiKeyScheme Handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.IFR.Security.Auth
{
    using CORE.Contracts;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Security.Claims;
    using System.Text.Encodings.Web;

    /// <summary>
    /// The ApiKey Scheme Handler.
    /// </summary>
    public class ApiKeySchemeHandler(
        IDbContext context,
        IOptionsMonitor<ApiKeySchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : AuthenticationHandler<ApiKeySchemeOptions>(options, logger, encoder)
    {
        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(Options.HeaderName, out var apiKeyHeaderValues)){ return AuthenticateResult.NoResult(); }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(providedApiKey)){ return AuthenticateResult.Fail("Invalid API Key provided."); }

            try{
                var apiKey = await context.ApiKeys.SingleOrDefaultAsync(x => x.Key == Guid.Parse(providedApiKey));
                if (apiKey == null){ return AuthenticateResult.Fail("Invalid API Key provided."); }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, apiKey.Name),
                    new Claim(ClaimTypes.NameIdentifier, $"{apiKey.ApiKeyId}"),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (FormatException){ return AuthenticateResult.Fail("Invalid API Key format."); }
        }
    }
}