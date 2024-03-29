﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delete.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the DeleteModel type.
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
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The delete model for user.
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
        /// Gets or sets the user.
        /// </summary>
        [BindProperty]
        public UserDto? UserDto { get; set; }

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

            var url = string.Format(ApiEndPoints.GetUser, id);
            var userDto = await this.ApiClient.Get<UserDto>(url);

            this.UserDto = userDto;

            if (this.UserDto == null){ return NotFound(); }

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
                var url = string.Format(ApiEndPoints.GetUser, id);
                var userDto = await this.ApiClient.Get<UserDto>(url);

                this.UserDto = userDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.UserDto != null){
                try{
                    var url = string.Format(ApiEndPoints.DeleteUsers, this.UserDto.UserName);
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