// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Register.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   The register model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Identity.Pages.Account
{
    using DTOs;
    using IFR.API;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Models.Areas.Identity.Account;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The register model.
    /// </summary>
    [Breadcrumb("Register")]
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The config app.
        /// </param>
        public RegisterModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        [BindProperty]
        public RegisterViewModel Input { get; set; }

        /// <summary>
        /// Gets or sets the return url.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// The on get.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        public void OnGet(string? returnUrl = null)
        {
            this.ReturnUrl = returnUrl!;
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

            var user = new UserDto
            {
                UserName = this.Input.Email,
                Password = this.Input.Password,
                Email = this.Input.Email,
                Nif = this.Input.Nif,
                FirstName = this.Input.FirstName,
                LastName = this.Input.LastName,
                PhoneNumber = this.Input.PhoneNumber,
                StreetAddress = this.Input.StreetAddress,
                City = this.Input.City,
                State = this.Input.State,
                Country = this.Input.Country,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            var url = ApiEndPoints.PostUsers;
            var result = await this.ApiClient.Post<UserDto>(url, user);

            if (result.Errors == null || !result.Errors.Any()){
                url = string.Format(ApiEndPoints.GetRoles);
                var roles = await this.ApiClient.Get<List<RoleDto>>(url);

                if (roles == null || roles.Count == 0) return Page();
                foreach (var roleDto in roles.Where(roleDto => roleDto.Name.ToLower().Equals("user"))){
                    url = string.Format(ApiEndPoints.PutUserRoles, result.Data!.UserName, roleDto.Id);
                    await this.ApiClient.Put(url);
                }

                return RedirectToPage("RegisterConfirmation", new { email = this.Input.Email });
            }

            foreach (var error in result.Errors!){ ModelState.AddModelError(string.Empty, error.Description); }
            return Page();
        }
    }
}