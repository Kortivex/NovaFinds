// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Index.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the IndexModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.Order
{
    using API.Filters;
    using DTOs;
    using IFR.API;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The index model for order.
    /// </summary>
    [Breadcrumb("Orders")]
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// The max menu pagination.
        /// </summary>
        private readonly int _maxMenuPagination;

        /// <summary>
        /// The shop config.
        /// </summary>
        private readonly IConfigurationSection _shopConfig;

        /// <summary>
        /// The shop crud config.
        /// </summary>
        private readonly IConfigurationSection _shopCrudConfig;

        /// <summary>
        /// The show pagination.
        /// </summary>
        private readonly int _showPagination;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The configuration.
        /// </param>
        public IndexModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);

            _shopConfig = config.GetSection("Config").GetSection("General").GetSection("Shop");
            _shopCrudConfig = _shopConfig.GetSection("Pages").GetSection("Crud");

            var pagination = _shopCrudConfig.GetSection("Pagination");
            _showPagination = int.Parse(pagination.GetSection("Show").Value!);
            _maxMenuPagination = int.Parse(pagination.GetSection("Max_Menu").Value!);
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public List<OrderDto> Orders { get; set; }

        /// <summary>
        /// The on get async.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnGetAsync([FromQuery] int page = 1)
        {
            ViewData["PaginationText"] = _shopCrudConfig.GetSection("SortFilter").GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);

            List<OrderDto>? totalOrdersDto;
            try{
                var url = string.Format(ApiEndPoints.GetOrdersSortFilters, _showPagination, "id", page);
                var ordersDto = await this.ApiClient.Get<List<OrderDto>>(url);

                url = string.Format(ApiEndPoints.GetOrders);
                totalOrdersDto = await this.ApiClient.Get<List<OrderDto>>(url);

                this.Orders = ordersDto!;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            ViewData["Pagination"] = new Paginator(
                totalOrdersDto!.Count,
                page,
                _showPagination,
                _maxMenuPagination);

            return Page();
        }
    }
}