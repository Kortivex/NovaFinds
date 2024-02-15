// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Create.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CreateModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.Product
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
    /// The create model for product.
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
        /// Gets or sets the product.
        /// </summary>
        [BindProperty]
        public ProductDto? Product { get; set; }

        /// <summary>
        /// The on get.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        public async Task<IActionResult> OnGet()
        {
            var url = string.Format(ApiEndPoints.GetCategories);
            var categoriesDto = await this.ApiClient.Get<List<CategoryDto>>(url);

            ViewData["CategoryId"] = new SelectList(categoriesDto, "Id", "Name");
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
            if (!ModelState.IsValid){
                var url = string.Format(ApiEndPoints.GetCategories);
                var categoriesDto = await this.ApiClient.Get<List<CategoryDto>>(url);

                ViewData["CategoryId"] = new SelectList(categoriesDto, "Id", "Name");
                return Page();
            }

            try{
                var url = string.Format(ApiEndPoints.PostProducts);
                await this.ApiClient.Post<ProductDto>(url, this.Product!);
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            return RedirectToPage("./Index");
        }
    }
}