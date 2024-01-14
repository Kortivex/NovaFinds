// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryMenu.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CategoryMenu type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Components
{
    using API;
    using DTOs;
    using IFR.Logger;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The category menu.
    /// </summary>
    public class CategoryMenu(IConfiguration configuration) : ViewComponent
    {
        private ApiClient _apiClient = new(configuration);

        /// <summary>
        /// The invoke async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Logger.Debug("Init Component");
            var categories = await _apiClient.Get<IEnumerable<CategoryDto>>(ApiEndPoints.GetCategories);
            return View(categories);
        }
    }
}
