// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryController.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Category controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using API;
    using DTOs;
    using IFR.Logger;
    using Microsoft.AspNetCore.Mvc;
    using NovaFinds.API.Filters;
    using SmartBreadcrumbs.Attributes;
    using System.Globalization;

    /// <summary>
    /// The category controller.
    /// </summary>
    [Route("Category")]
    public class CategoryController : MainController
    {
        /// <summary>
        /// The max List menu pagination.
        /// </summary>
        private int _maxListMenuPagination;

        /// <summary>
        /// The max Show menu pagination.
        /// </summary>
        private int _maxShowMenuPagination;

        /// <summary>
        /// The shop categories config.
        /// </summary>
        private readonly IConfigurationSection _shopCategoriesConfig;

        /// <summary>
        /// The shop category config.
        /// </summary>
        private readonly IConfigurationSection _shopCategoryConfig;

        /// <summary>
        /// The show pagination.
        /// </summary>
        private int _showPagination;

        /// <summary>
        /// The list pagination.
        /// </summary>
        private int _listPagination;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public CategoryController(IConfiguration configuration) : base(configuration)
        {
            var shopConfig = configuration.GetSection("Config").GetSection("General").GetSection("Shop");
            _shopCategoriesConfig = shopConfig.GetSection("Pages").GetSection("Categories");
            _shopCategoryConfig = shopConfig.GetSection("Pages").GetSection("Category");
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Breadcrumb("Categories")]
        public async Task<IActionResult> Index(int page = 1)
        {
            Logger.Debug("Init CategoryController Controller");
            // Categories List Page.
            var listPagination = _shopCategoriesConfig.GetSection("Pagination");
            _listPagination = int.Parse(listPagination.GetSection("Show").Value!, CultureInfo.InvariantCulture);
            _maxListMenuPagination = int.Parse(listPagination.GetSection("Max_Menu").Value!, CultureInfo.InvariantCulture);
            ViewData["PaginationListText"] = _shopCategoriesConfig.GetSection("SortFilter").GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);

            var categories = await ApiClient.Get<IEnumerable<CategoryDto>>(ApiEndPoints.GetCategories);

            var url = string.Format(ApiEndPoints.GetCategoriesSortFilters, _listPagination, page);
            ViewData["CategoriesPaginated"] = await ApiClient.Get<IEnumerable<CategoryDto>>(url);
            ViewData["Pagination"] = new Paginator(categories!.Count(), page, _listPagination, _maxListMenuPagination);

            return View("List");
        }

        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Breadcrumb("ViewData.Title")]
        [HttpGet("{categoryId:int}/[action]")]
        public async Task<IActionResult> Show(int categoryId, int page = 1)
        {
            // Categories Show Page.
            var showPagination = _shopCategoryConfig.GetSection("Pagination");
            _showPagination = int.Parse(showPagination.GetSection("Show").Value!, CultureInfo.InvariantCulture);
            _maxShowMenuPagination = int.Parse(showPagination.GetSection("Max_Menu").Value!, CultureInfo.InvariantCulture);
            ViewData["PaginationShowText"] = _shopCategoryConfig.GetSection("SortFilter").GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);

            // CATEGORY
            var url = string.Format(ApiEndPoints.GetCategory, categoryId);
            var category = await ApiClient.Get<CategoryDto>(url);
            ViewData["Name"] = category!.Name;
            ViewData["Id"] = categoryId.ToString(CultureInfo.InvariantCulture);

            // PRODUCTS
            var products = await ApiClient.Get<IEnumerable<CategoryDto>>(ApiEndPoints.GetProducts);

            url = string.Format(ApiEndPoints.GetProductsCategorySortFilter, categoryId, _showPagination, page);
            var productPag = await ApiClient.Get<IEnumerable<ProductDto>>(url);
            ViewData["ProductsPaginated"] = productPag;

            // PRODUCTS TOTAL
            ViewData["ProductsCount"] = productPag!.Count().ToString();

            // PAGINATION COMPONENT
            ViewData["Pagination"] = new Paginator(products!.Count(), page, _showPagination, _maxShowMenuPagination);

            return View("Show");
        }
    }
}