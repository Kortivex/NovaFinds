// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logout.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the LogoutModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Identity.Pages.Account
{
    using API;
    using CORE.Domain;
    using DTOs;
    using Faker;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using SmartBreadcrumbs.Attributes;
    using System.Security.Claims;

    /// <summary>
    /// The logout model.
    /// </summary>
    [Breadcrumb("Logout")]
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        /// <summary>
        /// The sign in manager.
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutModel"/> class.
        /// </summary>
        /// <param name="signInManager">
        /// The sign in manager.
        /// </param>
        /// <param name="config">
        /// The config app.
        /// </param>
        public LogoutModel(SignInManager<User> signInManager, IConfiguration config)
        {
            _signInManager = signInManager;
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// The on get.
        /// </summary>
        public void OnGet() {}

        /// <summary>
        /// The on post.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnPost(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            Logger.Debug("User logged out.");

            var url = string.Format(ApiEndPoints.GetCart, User.Identity!.Name);
            var cart = await this.ApiClient.Get<List<CartDto>>(url);
            if (cart!.Count != 0){
                url = string.Format(ApiEndPoints.DeleteCarts, cart[0].Id);
                this.ApiClient.Delete(url);
            }

            if (returnUrl != null) return LocalRedirect(returnUrl);
            return RedirectToPage();
        }
    }
}