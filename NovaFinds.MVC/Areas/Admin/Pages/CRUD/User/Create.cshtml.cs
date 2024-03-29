﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Create.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CreateModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.User
{
    using DTOs;
    using IFR.API;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The create model for user.
    /// </summary>
    [Breadcrumb("Create")]
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The configuration.
        /// </param>
        public CreateModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        [BindProperty]
        public UserDto? UserDto { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        [BindProperty]
        public RoleDto? RoleDto { get; set; }

        /// <summary>
        /// The on get.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var url = string.Format(ApiEndPoints.GetRoles);
            var rolesDto = await this.ApiClient.Get<List<RoleDto>>(url);

            ViewData["RoleId"] = new SelectList(rolesDto, "Id", "Name");

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
            try{
                this.UserDto!.EmailConfirmed = true;
                this.UserDto!.PhoneNumberConfirmed = true;
                this.UserDto!.IsActive = true;
                this.UserDto!.UserName = this.UserDto!.Email;

                var url = string.Format(ApiEndPoints.PostUsers);
                var result = await this.ApiClient.Post<UserDto>(url, this.UserDto!);
                var errs = result.Errors;
                if (errs != null){
                    foreach (var apiError in errs){ ModelState.AddModelError(string.Empty, apiError.Description); }

                    url = string.Format(ApiEndPoints.GetRoles);
                    var rolesDto = await this.ApiClient.Get<List<RoleDto>>(url);

                    ViewData["RoleId"] = new SelectList(rolesDto, "Id", "Name");

                    return Page();
                }

                url = string.Format(ApiEndPoints.PutUserRoles, this.UserDto.UserName, this.RoleDto!.Id);
                await this.ApiClient.Put(url);
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            return RedirectToPage("./Index");
        }
    }
}