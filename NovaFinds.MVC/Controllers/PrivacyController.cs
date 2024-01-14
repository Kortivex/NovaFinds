// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrivacyController.cs" company="">
//
// </copyright>
// <summary>
//   Defines the PrivacyController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using IFR.Logger;
    using Microsoft.AspNetCore.Mvc;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The privacy controller.
    /// </summary>
    public class PrivacyController : MainController
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivacyController"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public PrivacyController(IConfiguration configuration) : base(configuration)
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
                .GetSection("Privacy").GetSection("Date").Value;
            ViewData["ShopUrl"] = shopConfig.GetSection("Url").Value;
            return View("Privacy");
        }
    }
}