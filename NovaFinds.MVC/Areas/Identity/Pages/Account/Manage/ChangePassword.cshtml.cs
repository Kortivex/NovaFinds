// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangePassword.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ChangePasswordModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Identity.Pages.Account.Manage
{
    using DTOs;
    using IFR.API;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Models.Areas.Identity.Account.Manage;
    using SmartBreadcrumbs.Attributes;

    [Breadcrumb("Change Password")]
    [Authorize(Roles = "User,Admin")]
    public class ChangePasswordModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The config app.
        /// </param>
        public ChangePasswordModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        [BindProperty]
        public ChangePasswordViewModel Input { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// The on get async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnGetAsync()
        {
            this.StatusMessage = "";
            // API call to get user info.
            var url = string.Format(ApiEndPoints.GetUsersEmailFilter, User.Identity!.Name);
            var users = await this.ApiClient.Get<List<UserDto>>(url);

            if (users == null || users.Count == 0){ return NotFound($"Unable to load user with ID '{User.Identity!.Name}'."); }
            return Page();
        }

        /// <summary>
        /// The on post async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // API call to get user info.
            var url = string.Format(ApiEndPoints.GetUsersEmailFilter, User.Identity!.Name);
            var users = await this.ApiClient.Get<List<UserDto>>(url);
            if (users == null || users.Count == 0){ return NotFound($"Unable to load user with ID '{User.Identity!.Name}'."); }
            var user = users[0];

            user.Password = this.Input.NewPassword;
            // API call to update user info.
            url = string.Format(ApiEndPoints.PutUsers, User.Identity!.Name);
            var result = await this.ApiClient.Put<UserDto>(url, user);

            if (result.Errors == null || !result.Errors.Any()){
                Logger.Info("User changed their password successfully.");
                this.StatusMessage = "Your password has been changed.";
                return Page();
            }

            foreach (var error in result.Errors!){ ModelState.AddModelError(string.Empty, error.Description); }
            return Page();
        }
    }
}