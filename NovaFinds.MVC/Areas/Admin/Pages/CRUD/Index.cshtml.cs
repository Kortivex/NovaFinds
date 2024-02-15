// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Index.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the IndexModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The index model for init crud over the entities.
    /// </summary>
    [Breadcrumb("Shop Management")]
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// The on get async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
    }
}