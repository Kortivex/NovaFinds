// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Edit.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the EditModel type.
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
    /// The edit model for user.
    /// </summary>
    [Breadcrumb("Edit")]
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The configuration.
        /// </param>
        public EditModel(IConfiguration config)
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
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            List<RoleDto>? rolesDto;
            try{
                var url = string.Format(ApiEndPoints.GetUser, id);
                var userDto = await this.ApiClient.Get<UserDto>(url);

                url = string.Format(ApiEndPoints.GetUserRoles, userDto!.UserName);
                var roleDto = await this.ApiClient.Get<List<RoleDto>>(url);

                url = string.Format(ApiEndPoints.GetRoles);
                rolesDto = await this.ApiClient.Get<List<RoleDto>>(url);

                this.UserDto = userDto;
                this.RoleDto = rolesDto![0];
                if (roleDto!.Count != 0){ this.RoleDto = roleDto[0]; }
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.UserDto == null) return NotFound();
            if (this.RoleDto == null) return NotFound();

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

                var url = string.Format(ApiEndPoints.PutUsers, this.UserDto!.UserName);
                var result = await this.ApiClient.Put<UserDto>(url, this.UserDto!);
                var errs = result.Errors;
                if (errs != null){
                    foreach (var apiError in errs){ ModelState.AddModelError(string.Empty, apiError.Description); }

                    url = string.Format(ApiEndPoints.GetRoles);
                    var rolesDto = await this.ApiClient.Get<List<RoleDto>>(url);

                    ViewData["RoleId"] = new SelectList(rolesDto, "Id", "Name");

                    return Page();
                }

                url = string.Format(ApiEndPoints.GetUserRoles, this.UserDto.UserName);
                var userRoles = await this.ApiClient.Get<List<RoleDto>>(url);

                if (userRoles!.Count >= 1){
                    foreach (var role in userRoles){
                        url = string.Format(ApiEndPoints.DeleteUserRoles, this.UserDto.UserName, role.Id);
                        this.ApiClient.Delete(url);
                    }
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