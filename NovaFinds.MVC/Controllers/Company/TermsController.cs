﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TermsController.cs" company="">
//
// </copyright>
// <summary>
//   The terms controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers.Company
{
    using IFR.Logger;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The terms controller.
    /// </summary>
    public class TermsController : Controller
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TermsController"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public TermsController(IConfiguration configuration)
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
            ViewData["ShopLegalDate"] = shopConfig.GetSection("Documents").GetSection("Legal").GetSection("Terms")
                .GetSection("Date").Value;
            ViewData["ShopUrl"] = shopConfig.GetSection("Url").Value;
            ViewData["ShopFullUrl"] = shopConfig.GetSection("FullUrl").Value;
            return View("Terms");
        }
    }
}