// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delete.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the DeleteModel type.
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
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The delete model for role.
    /// </summary>
    [Breadcrumb("Delete")]
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The configuration.
        /// </param>
        public DeleteModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// Gets or sets the product.
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
            if (id == null){ return NotFound(); }

            var url = string.Format(ApiEndPoints.GetRole, id);
            var roleDto = await this.ApiClient.Get<RoleDto>(url);

            this.Role = roleDto;

            if (this.Role == null){ return NotFound(); }

            return Page();
        }

        /// <summary>
        /// The on post async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null){ return NotFound(); }

            try{
                var url = string.Format(ApiEndPoints.GetRole, id);
                var roleDto = await this.ApiClient.Get<RoleDto>(url);

                this.Role = roleDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.Role != null){
                try{
                    var url = string.Format(ApiEndPoints.DeleteRole, id);
                    this.ApiClient.Delete(url);
                }
                catch (Exception exception){
                    Logger.Error(exception.Message);
                    return RedirectToPage("~/");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}