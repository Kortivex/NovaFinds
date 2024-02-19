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
    using Mailjet.Client;
    using Mailjet.Client.Resources;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The email sender.
    /// </summary>
    public class EmailService : IEmailSender
    {
        /// <summary>
        /// The MailJet api key.
        /// </summary>
        private readonly string _apiKey;

        /// <summary>
        /// The MailJet secret.
        /// </summary>
        private readonly string _secret;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration.GetSection("Api").GetSection("ApiKeys").GetSection("MailJetApi").Value!;
            _secret = configuration.GetSection("Api").GetSection("ApiKeys").GetSection("MailJetSecret").Value!;
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
        public Task<MailjetResponse> SendEmailAsync(
            string email,
            string subject,
            string message,
            string from,
            string name = "")
        {
            return ConfigEmailToSend(_apiKey, _secret, subject, message, email, from, name);
        }

        /// <summary>
        /// The config email to send.
        /// </summary>
        /// <param name="apiKey">
        /// The api key.
        /// </param>
        /// <param name="secret">
        /// The secret key.
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
        private async Task<MailjetResponse> ConfigEmailToSend(
            string apiKey,
            string secret,
            string subject,
            string message,
            string email,
            string from = "",
            string name = "")
        {
            var client = new MailjetClient(apiKey, secret);
            var request = new MailjetRequest
                {
                    Resource = SendV31.Resource,
                }
                .Property(Send.Messages,
                    new JArray
                    {
                        new JObject
                        {
                            {
                                "From", new JObject
                                {
                                    { "Email", from },
                                    { "Name", name }
                                }
                            },
                            {
                                "To", new JArray
                                {
                                    new JObject
                                    {
                                        { "Email", email },
                                        { "Name", email }
                                    }
                                }
                            },
                            { "Subject", subject },
                            { "TextPart", message },
                            { "HTMLPart", message }
                        }
                    });

            var response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode){
                Logger.Debug($"Email sent successfully, Total: {response.GetTotal()}, Count: {response.GetCount()}");
                Logger.Debug(response.GetData().ToString());
            }
            else{ Logger.Debug($"An error occurred: StatusCode = {response.StatusCode}, ErrorInfo = {response.GetErrorInfo()}"); }

            return response;
        }
    }
}