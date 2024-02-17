// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutController.cs" company="">
//
// </copyright>
// <summary>
//   Defines the AboutController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using IFR.Logger;
    using Microsoft.AspNetCore.Mvc;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The about shop controller.
    /// </summary>
    [Route("About")]
    public class AboutController(IConfiguration config) : MainController(config)
    {
        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [Breadcrumb("ViewData.Title")]
        public IActionResult Index()
        {
            Logger.Debug("Init AboutController Controller");
            return View("About");
        }
    }
}