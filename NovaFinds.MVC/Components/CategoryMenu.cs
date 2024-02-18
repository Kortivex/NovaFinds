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
    using DTOs;
    using IFR.API;
    using IFR.Logger;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The category menu.
    /// </summary>
    public class CategoryMenu(IConfiguration configuration) : ViewComponent
    {
        private readonly ApiClient _apiClient = new(configuration);

        /// <summary>
        /// The invoke async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Logger.Debug("Init CategoryMenu Component");
            var categories = await _apiClient.Get<IEnumerable<CategoryDto>>(ApiEndPoints.GetCategories);
            return View(categories);
        }
    }
}
