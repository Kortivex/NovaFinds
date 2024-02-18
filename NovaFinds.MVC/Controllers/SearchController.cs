// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchController.cs" company="">
//
// </copyright>
// <summary>
//   Defines the SearchController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using DTOs;
    using IFR.API;
    using IFR.Logger;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The search controller.
    /// </summary>
    [Route("Search")]
    public class SearchController : MainController
    {

        /// <summary>
        /// The shop search size.
        /// </summary>
        private readonly int _shopSearchSize;

        public SearchController(IConfiguration configuration) : base(configuration)
        {
            var shopConfig = configuration.GetSection("Config").GetSection("General").GetSection("Shop");
            var shopSearchSizeValue = shopConfig.GetSection("Pages").GetSection("Home").GetSection("Search").GetSection("Size").Value;
            _shopSearchSize = int.Parse(shopSearchSizeValue!);
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="product">
        /// The product.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("Product")]
        public async Task<ActionResult<ICollection<ProductDto>>> Index(string product)
        {
            Logger.Debug("Init SearchController Controller");
            var url = string.Format(ApiEndPoints.GetProductsNameSortFilters, product, _shopSearchSize);
            var response = await ApiClient.Get<IEnumerable<ProductDto>>(url);

            return Json(response);
        }
    }
}