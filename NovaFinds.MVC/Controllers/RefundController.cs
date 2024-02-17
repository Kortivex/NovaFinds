// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RefundController.cs" company="">
//
// </copyright>
// <summary>
//   Defines the RefundController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using IFR.Logger;
    using Microsoft.AspNetCore.Mvc;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The refund controller.
    /// </summary>
    [Route("Refund")]
    public class RefundController : MainController
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefundController"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public RefundController(IConfiguration configuration) : base(configuration)
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
            Logger.Debug("Init RefundController Controller");
            var shopConfig = _configuration.GetSection("Config").GetSection("General").GetSection("Shop");
            var streetRefund = shopConfig.GetSection("Streets").GetSection("Refunds");
            ViewData["ShopLegalDate"] = shopConfig.GetSection("Documents").GetSection("Legal").GetSection("Refund")
                .GetSection("Date").Value;
            ViewData["ShopUrl"] = shopConfig.GetSection("Url").Value;
            ViewData["ShopStreetRefunds"] = new Dictionary<string, string>
            {
                { "Root", streetRefund.GetSection("Root").Value! },
                { "City", streetRefund.GetSection("City").Value! },
                { "State", streetRefund.GetSection("State").Value! },
                { "Postal", streetRefund.GetSection("Postal").Value! },
                { "Country", streetRefund.GetSection("Country").Value! }
            };
            return View("Refund");
        }
    }
}