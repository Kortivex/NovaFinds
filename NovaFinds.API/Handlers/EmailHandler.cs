﻿// --------------------------------------------------------------------------------------------------------------------
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
    using DTOs;
    using IFR.Logger;
    using IFR.Security.Auth;
    using Microsoft.AspNetCore.Authorization;
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

            await _emailService.SendEmailAsync(emailDto!.Email, emailDto.Subject, emailDto.Message,  emailDto.From, emailDto.Name);
            return TypedResults.Created($"/emails");
        }
    }
}