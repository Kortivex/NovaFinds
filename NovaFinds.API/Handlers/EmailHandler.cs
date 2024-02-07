// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailHandler.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Email Handler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.API.Handlers
{
    using Application.Services;
    using Auth;
    using DTOs;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Net.Http.Headers;
    using System.Text.Json;

    [Authorize(AuthenticationSchemes = ApiKeySchemeOptions.AuthenticateScheme)]
    public class EmailHandler(IConfiguration configuration)
    {
        /// <summary>
        /// The email service.
        /// </summary>
        private readonly EmailService _emailService = new(configuration);

        public async Task<IResult> PostEmails(HttpRequest request)
        {
            Logger.Debug("Post Email Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var emailDto = JsonSerializer.Deserialize<EmailDto>(body);

            var response = await _emailService.SendEmailAsync(emailDto!.Email, emailDto.Subject, emailDto.Message,  emailDto.From, emailDto.Name);

            return TypedResults.Content(response.GetData().ToString(), new MediaTypeHeaderValue("application/json"));
        }
    }
}