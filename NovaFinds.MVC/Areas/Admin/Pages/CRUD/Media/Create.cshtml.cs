// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Create.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CreateModel type.
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
    /// The create model for media.
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
        /// Gets or sets the image.
        /// </summary>
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        /// <summary>
        /// Gets or sets the media.
        /// </summary>
        [BindProperty]
        public ProductImageDto? Image { get; set; }

        /// <summary>
        /// The on get.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var url = string.Format(ApiEndPoints.GetProducts);
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
                // The image is converted to base64
                if (this.Image != null){
                    var memoryStream = new MemoryStream();
                    await this.ImageFile.OpenReadStream().CopyToAsync(memoryStream);
                    var base64Image = Convert.ToBase64String(memoryStream.ToArray());

                    this.Image.Image = $"data:image/jpeg;base64,{base64Image}";
                }

                var url = string.Format(ApiEndPoints.PostImages);
                await this.ApiClient.Post<ProductImageDto>(url, this.Image!);
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            return RedirectToPage("./Index");
        }
    }
}