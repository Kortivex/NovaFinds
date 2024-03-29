﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterConfirmation.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the RegisterConfirmationModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Identity.Pages.Account
{
    using DTOs;
    using IFR.API;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The register confirmation model.
    /// </summary>
    [Breadcrumb("Register Confirmation")]
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterConfirmationModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The config app.
        /// </param>
        public RegisterConfirmationModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The on get async.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnGetAsync(string? email)
        {
            if (email == null){ return RedirectToPage("/Index"); }

            var url = string.Format(ApiEndPoints.GetUsersEmailFilter, email);
            var users = await this.ApiClient.Get<List<UserDto>>(url);
            if (users!.Count != 1){ return NotFound($"Unable to load user with email '{email}'."); }

            this.Email = email;
            ViewData["Email"] = this.Email;

            return Page();
        }
    }
}