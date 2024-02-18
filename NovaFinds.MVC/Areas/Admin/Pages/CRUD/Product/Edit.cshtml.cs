// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Edit.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the EditModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.Product
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
    /// The edit model for product.
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
        /// Gets or sets the product.
        /// </summary>
        [BindProperty]
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

            List<CategoryDto>? categoriesDto;
            try{
                var url = string.Format(ApiEndPoints.GetProduct, id);
                var productDto = await this.ApiClient.Get<ProductDto>(url);

                url = string.Format(ApiEndPoints.GetCategories);
                categoriesDto = await this.ApiClient.Get<List<CategoryDto>>(url);

                this.Product = productDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.Product == null) return NotFound();

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
                var url = string.Format(ApiEndPoints.GetProduct, this.Product!.Id);
                var productDto = await this.ApiClient.Get<ProductDto>(url);

                if (productDto != null){
                    productDto.Id = this.Product.Id;
                    productDto.Name = this.Product.Name;
                    productDto.Brand = this.Product.Brand;
                    productDto.Price = this.Product.Price;
                    productDto.CategoryId = this.Product.CategoryId;
                    productDto.Description = this.Product.Description;
                    productDto.Stock = this.Product.Stock;
                }

                url = string.Format(ApiEndPoints.PutProducts, this.Product!.Id);
                await this.ApiClient.Put<ProductDto>(url, productDto!);

                if (productDto == null) return NotFound();
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            return RedirectToPage("./Index");
        }
    }
}