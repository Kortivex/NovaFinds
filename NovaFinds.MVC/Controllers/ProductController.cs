// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductController.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ProductController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using DTOs;
    using IFR.API;
    using IFR.Logger;
    using Microsoft.AspNetCore.Mvc;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The product controller.
    /// </summary>
    [Route("Product")]
    public class ProductController : MainController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        public ProductController(IConfiguration configuration) : base(configuration) {}

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [Breadcrumb("Products")]
        public IActionResult Index()
        {
            Logger.Debug("Init ProductController Controller");
            return RedirectToAction("Index", "Category");
        }

        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="productId">
        /// The product id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Breadcrumb("ViewData.Title")]
        [HttpGet("{productId:int}/Show")]
        public async Task<IActionResult> Show(int productId)
        {
            var url = string.Format(ApiEndPoints.GetProduct, productId);
            var product = await ApiClient.Get<ProductDto>(url);

            ViewData["Product"] = product;
            ViewData["ProductTax"] = product!.Price + product.Price * (21.0 / 100.0);

            return View("Show");
        }
    }
}