// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShippingAddress.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ShippingAddressModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Identity.Pages.Account.Manage
{
    using API;
    using DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Models.Areas.Identity.Account.Manage;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The shipping address model.
    /// </summary>
    [Breadcrumb("Shipping Address")]
    public class ShippingAddressModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingAddressModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The config app.
        /// </param>
        public ShippingAddressModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        [BindProperty]
        public ShippingAddressViewModel Input { get; set; }

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
            await LoadAsync(users[0]);
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
            // API call to get user info.
            var url = string.Format(ApiEndPoints.GetUsersEmailFilter, User.Identity!.Name);
            var users = await this.ApiClient.Get<List<UserDto>>(url);
            if (users == null || users.Count == 0){ return NotFound($"Unable to load user with ID '{User.Identity!.Name}'."); }
            var user = users[0];

            if (!ModelState.IsValid){
                await LoadAsync(user);
                return Page();
            }

            if (this.Input.ShippingStreetAddress != user.StreetAddress){ user.StreetAddress = this.Input.ShippingStreetAddress; }
            if (this.Input.ShippingCity != user.City){ user.City = this.Input.ShippingCity; }
            if (this.Input.ShippingState != user.State){ user.State = this.Input.ShippingState; }
            if (this.Input.ShippingCountry != user.Country){ user.Country = this.Input.ShippingCountry; }

            // API call to update user info.
            url = string.Format(ApiEndPoints.PutUsers, User.Identity!.Name);
            var result = await this.ApiClient.Put<UserDto>(url, user);

            if (result.Errors == null || !result.Errors.Any()){
                this.StatusMessage = "Your shipping address has been updated";
                return Page();
            }

            foreach (var error in result.Errors!){ ModelState.AddModelError(string.Empty, error.Description); }
            return Page();
        }

        /// <summary>
        /// The load async.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task LoadAsync(UserDto user)
        {
            var url = string.Format(ApiEndPoints.GetUsersEmailFilter, User.Identity!.Name);
            var users = await this.ApiClient.Get<List<UserDto>>(url);
            if (users != null || users!.Count != 0){
                var userRetrieved = users[0];
                this.Input = new ShippingAddressViewModel
                {
                    ShippingStreetAddress = userRetrieved.StreetAddress,
                    ShippingCity = userRetrieved.City,
                    ShippingState = userRetrieved.State,
                    ShippingCountry = userRetrieved.Country
                };
            }
        }
    }
}