// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailService.cs" company="">
//
// </copyright>
// <summary>
//   Defines the EmailService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Services
{
    using IFR.Logger;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    /// <summary>
    /// The email sender.
    /// </summary>
    public class EmailService : IEmailSender
    {
        /// <summary>
        /// The sendgrid api key.
        /// </summary>
        private readonly string _apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration.GetSection("ApiKeys").GetSection("SendGrid").Value!;
        }

        /// <summary>
        /// The send email async.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="subject">
        /// The subject.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return SendEmailAsync(email, subject, message, string.Empty, string.Empty);
        }

        /// <summary>
        /// The send email async.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="subject">
        /// The subject.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<Response> SendEmailAsync(
            string email,
            string subject,
            string message,
            string from,
            string name = "")
        {
            return ConfigEmailToSend(_apiKey, subject, message, email, from, name);
        }

        /// <summary>
        /// The config email to send.
        /// </summary>
        /// <param name="apiKey">
        /// The api key.
        /// </param>
        /// <param name="subject">
        /// The subject.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<Response> ConfigEmailToSend(
            string apiKey,
            string subject,
            string message,
            string email,
            string from = "",
            string name = "")
        {
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(new EmailAddress(from, name), new EmailAddress(email, ""), subject, message, message);
            var response = await client.SendEmailAsync(msg);

            Logger.Debug(response.StatusCode.ToString());
            Logger.Debug(await response.Body.ReadAsStringAsync());
            Logger.Debug(response.Headers.ToString());

            return response;
        }
    }
}