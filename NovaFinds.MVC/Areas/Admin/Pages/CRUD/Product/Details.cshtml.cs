// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Details.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the DetailsModel type.
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
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The details model for product.
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
        /// Gets or sets the product.
        /// </summary>
        public ProductDto? Product { get; set; }
        
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public CategoryDto? Category { get; set; }

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
                var url = string.Format(ApiEndPoints.GetProduct, id);
                var productDto = await this.ApiClient.Get<ProductDto>(url);
                
                url = string.Format(ApiEndPoints.GetCategory, productDto!.CategoryId);
                var categoryDto = await this.ApiClient.Get<CategoryDto>(url);

                this.Product = productDto;
                this.Category = categoryDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.Product == null){ return NotFound(); }

            return Page();
        }
    }
}