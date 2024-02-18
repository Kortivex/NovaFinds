// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Edit.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the EditModel type.
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
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The edit model for media.
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
        /// Gets or sets the image.
        /// </summary>
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        /// <summary>
        /// Gets or sets the media.
        /// </summary>
        [BindProperty]
        public ProductImageDto? Image { get; set; }

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

            string url;
            try{
                url = string.Format(ApiEndPoints.GetImage, id);
                var productImageDto = await this.ApiClient.Get<ProductImageDto>(url);

                this.Image = productImageDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.Image == null) return NotFound();

            url = string.Format(ApiEndPoints.GetProducts);
            var productsDto = await this.ApiClient.Get<List<ProductDto>>(url);

            ViewData["ProductId"] = new SelectList(productsDto, "Id", "Name");

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
                var url = string.Format(ApiEndPoints.GetImage, this.Image!.Id);
                var productImageDto = await this.ApiClient.Get<ProductImageDto>(url);

                if (productImageDto != null){
                    productImageDto.Id = this.Image!.Id;
                    productImageDto.Description = this.Image!.Description;
                    productImageDto.ProductId = this.Image.ProductId;

                    // The image is converted to base64
                    if (this.Image != null){
                        var memoryStream = new MemoryStream();
                        await this.ImageFile!.OpenReadStream().CopyToAsync(memoryStream);
                        var base64Image = Convert.ToBase64String(memoryStream.ToArray());

                        productImageDto.Image = $"data:image/jpeg;base64,{base64Image}";
                    }
                    
                    url = string.Format(ApiEndPoints.PutImage, this.Image!.Id);
                    await this.ApiClient.Put<ProductImageDto>(url, productImageDto);
                }

                if (productImageDto == null) return NotFound();
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            return RedirectToPage("./Index");
        }
    }
}