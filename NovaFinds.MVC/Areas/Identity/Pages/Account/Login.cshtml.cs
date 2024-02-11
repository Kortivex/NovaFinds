// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Login.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the LoginModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Identity.Pages.Account
{
    using API;
    using CORE.Domain;
    using DTOs;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Models.Areas.Identity.Account;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The login model.
    /// </summary>
    [Breadcrumb("Login")]
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// The sign in manager.
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModel"/> class.
        /// </summary>
        /// <param name="signInManager">
        /// The sign in manager.
        /// </param>
        /// <param name="config">
        /// The config app.
        /// </param>
        public LoginModel(SignInManager<User> signInManager, IConfiguration config)
        {
            _signInManager = signInManager;
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        [BindProperty]
        public LoginViewModel Input { get; set; }

        /// <summary>
        /// Gets or sets the return url.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// The on get async.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
                ModelState.AddModelError(string.Empty, this.ErrorMessage);

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ReturnUrl = returnUrl;
        }

        /// <summary>
        /// The on post async.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid) return Page();

            // API call to get user info.
            var url = string.Format(ApiEndPoints.GetUsersEmailFilter, this.Input.Email);
            var users = await this.ApiClient.Get<List<UserDto>>(url);

            if (!users![0].IsActive){
                ModelState.AddModelError(string.Empty, "Not Allowed to Login");
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                this.Input.Email,
                this.Input.Password,
                this.Input.RememberMe,
                false);
            if (result.Succeeded){
                Logger.Debug("User Logged In");

                if (HttpContext.Session.Keys.Contains("tempUser")){
                    var tempUsername = HttpContext.Session.GetString("tempUser");

                    url = string.Format(ApiEndPoints.GetUserOrders, tempUsername);
                    var orders = await this.ApiClient.Get<List<OrderDto>>(url);

                    if (orders is not { Count: > 0 }){
                        url = string.Format(ApiEndPoints.GetCart, tempUsername);
                        var cart = await this.ApiClient.Get<List<CartDto>>(url);
                        if (cart!.Count != 0){
                            url = string.Format(ApiEndPoints.DeleteCarts, cart[0].Id);
                            this.ApiClient.Delete(url);
                        }
                        url = string.Format(ApiEndPoints.DeleteUsers, tempUsername);
                        this.ApiClient.Delete(url);
                        HttpContext.Session.Remove("tempUser");
                    }
                }
                // API call to get cart info.
                url = string.Format(ApiEndPoints.GetCart, users[0].UserName);
                var userCart = await this.ApiClient.Get<List<CartDto>>(url);
                if (userCart!.Count == 0) return LocalRedirect(returnUrl);
                // Cart items are restored if the set date has not been exempted.
                if (!(DateTime.Now.Subtract(userCart[0].Date!.Value).Days / (365.25 / 12) <= 1)){ this.ApiClient.Delete(string.Format(ApiEndPoints.DeleteCarts, userCart[0].Id)); }

                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid Login attempt");
            return Page();
        }
    }
}