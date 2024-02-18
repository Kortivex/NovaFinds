// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Index.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the IndexModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Identity.Pages.Account.Manage
{
    using DTOs;
    using IFR.API;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Models.Areas.Identity.Account.Manage;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The index model.
    /// </summary>
    [Breadcrumb("Manage Profile")]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The config app.
        /// </param>
        public IndexModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        [BindProperty]
        public IndexViewModel Input { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

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

            if (this.Input.PhoneNumber != user.PhoneNumber){ user.PhoneNumber = this.Input.PhoneNumber; }
            if (this.Input.Nif != user.Nif){ user.Nif = this.Input.Nif; }
            if (this.Input.FirstName != user.FirstName){ user.FirstName = this.Input.FirstName; }
            if (this.Input.LastName != user.LastName){ user.LastName = this.Input.LastName; }
            if (this.Input.StreetAddress != user.StreetAddress){ user.StreetAddress = this.Input.StreetAddress; }
            if (this.Input.City != user.City){ user.City = this.Input.City; }
            if (this.Input.State != user.State){ user.State = this.Input.State; }
            if (this.Input.Country != user.Country){ user.Country = this.Input.Country; }

            // API call to update user info.
            url = string.Format(ApiEndPoints.PutUsers, User.Identity!.Name);
            var result = await this.ApiClient.Put<UserDto>(url, user);

            if (result.Errors == null || !result.Errors.Any()){
                this.Username = User.Identity.Name!;
                this.StatusMessage = "Your profile has been updated";
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
                this.Username = userRetrieved.UserName;
                this.Input = new IndexViewModel
                {
                    PhoneNumber = userRetrieved.PhoneNumber,
                    Nif = userRetrieved.Nif,
                    FirstName = userRetrieved.FirstName,
                    LastName = userRetrieved.LastName,
                    StreetAddress = userRetrieved.StreetAddress,
                    City = userRetrieved.City,
                    State = userRetrieved.State,
                    Country = userRetrieved.Country
                };
            }
        }
    }
}