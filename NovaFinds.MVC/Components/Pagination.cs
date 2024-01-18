﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pagination.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Pagination type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Components
{
    using IFR.Logger;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using NovaFinds.API.Filters;

    /// <summary>
    /// The pagination.
    /// </summary>
    public class Pagination : ViewComponent
    {
        /// <summary>
        /// The invoke async.
        /// </summary>
        /// <param name="paginator">
        /// The paginator.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<IViewComponentResult> InvokeAsync(Paginator paginator, Dictionary<string, string> text)
        {
            Logger.Debug("Init Component");
            return Task.FromResult<IViewComponentResult>(View(new Dictionary<string, object> { { "Paginator", paginator }, { "Text", text } }));
        }
    }
}