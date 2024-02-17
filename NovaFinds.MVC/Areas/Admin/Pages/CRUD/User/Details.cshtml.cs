// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Details.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the DetailsModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.User
{
    using API;
    using DTOs;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The details model for user.
    /// </summary>
    [Breadcrumb("Details")]
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The configuration.
        /// </param>
        public DetailsModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public UserDto? UserDto { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public List<RoleDto>? Role { get; set; }

        /// <summary>
        /// The on get async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null){ return NotFound(); }

            try{
                var url = string.Format(ApiEndPoints.GetUser, id);
                var userDto = await this.ApiClient.Get<UserDto>(url);

                url = string.Format(ApiEndPoints.GetUserRoles, userDto!.UserName);
                var roleDto = await this.ApiClient.Get<List<RoleDto>>(url);

                this.UserDto = userDto;
                this.Role = roleDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.UserDto == null){ return NotFound(); }
            if (this.Role == null){ return NotFound(); }

            return Page();
        }
    }
}