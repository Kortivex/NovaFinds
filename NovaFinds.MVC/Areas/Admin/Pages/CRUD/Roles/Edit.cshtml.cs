// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Edit.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the EditModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.Roles
{
    using API;
    using DTOs;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The edit model for role.
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
        /// Gets or sets the role.
        /// </summary>
        [BindProperty]
        public RoleDto? Role { get; set; }

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
            if (id == null) return NotFound();

            try{
                var url = string.Format(ApiEndPoints.GetRole, id);
                var roleDto = await this.ApiClient.Get<RoleDto>(url);

                this.Role = roleDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.Role == null) return NotFound();

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
            if (!ModelState.IsValid){ return Page(); }

            try{
                var url = string.Format(ApiEndPoints.GetRole, this.Role!.Id);
                var roleDto = await this.ApiClient.Get<RoleDto>(url);

                if (roleDto != null){
                    roleDto.Id = this.Role.Id;
                    roleDto.Name = this.Role.Name;
                }

                url = string.Format(ApiEndPoints.PutRole, this.Role!.Id);
                await this.ApiClient.Put<ProductDto>(url, roleDto!);

                if (roleDto == null) return NotFound();
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            return RedirectToPage("./Index");
        }
    }
}