// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
//
// </copyright>
// <summary>
//   The home controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using API;
    using DTOs;
    using IFR.Logger;
    using Microsoft.AspNetCore.Mvc;
    using SmartBreadcrumbs.Attributes;
    using System.Globalization;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : MainController
    {
        /// <summary>
        /// The shop home sections.
        /// </summary>
        private readonly IConfiguration _shopHomeSections;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public HomeController(IConfiguration configuration) : base(configuration)
        {
            IConfiguration config = configuration.GetSection("Config").GetSection("General").GetSection("Shop");
            _shopHomeSections = config.GetSection("Pages").GetSection("Home").GetSection("Items");
        }

        /// <summary>
        /// The error view generator.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// The index of page to show.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [DefaultBreadcrumb("Home")]
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.Keys.Contains("Date"))
                HttpContext.Session.SetString("Date", DateTime.Now.ToLongDateString());

            var homeSectionsSize = int.Parse(_shopHomeSections.GetSection("Size").Value!, CultureInfo.InvariantCulture);
            Logger.Debug("Doing request to /products");
            var productsSorted = await ApiClient.Get<IEnumerable<ProductDto>>(string.Format(ApiEndPoints.GetProducts, homeSectionsSize, "image"));

            ViewData["ProductsLatest"] = productsSorted.OrderByDescending(product => product.Id).ToList();
            ViewData["ProductsCheapest"] = productsSorted.OrderBy(product => product.Price).ToList();

            return View("Home");
        }
    }
}