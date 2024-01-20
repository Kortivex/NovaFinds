// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShippingController.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ShippingController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers.Help
{
    using IFR.Logger;
    using Microsoft.AspNetCore.Mvc;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The shipping controller.
    /// </summary>
    [Route("Shipping")]
    public class ShippingController : MainController
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingController"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public ShippingController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [Breadcrumb("ViewData.Title")]
        public IActionResult Index()
        {
            Logger.Debug("Init Controller");
            var shopConfig = _configuration.GetSection("Config").GetSection("General").GetSection("Shop");
            ViewData["ShopLegalDate"] = shopConfig.GetSection("Documents").GetSection("Legal")
                .GetSection("Shipping").GetSection("Date").Value;
            return View("Shipping");
        }
    }
}