// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Details.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the DetailsModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.Media
{
    using DTOs;
    using IFR.API;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The details model for media.
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
        /// Gets or sets the image.
        /// </summary>
        public ProductImageDto? Image { get; set; }

        public ProductDto? Product { get; set; }

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
                var url = string.Format(ApiEndPoints.GetImage, id);
                var productImageDto = await this.ApiClient.Get<ProductImageDto>(url);

                url = string.Format(ApiEndPoints.GetProduct, productImageDto!.ProductId);
                var productDto = await this.ApiClient.Get<ProductDto>(url);

                this.Image = productImageDto;
                this.Product = productDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.Image == null) return NotFound();
            return Page();
        }
    }
}