// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Orders.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the OrdersModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Identity.Pages.Account.Manage
{
    using API;
    using CORE.Domain;
    using DTOs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The orders model.
    /// </summary>
    [Breadcrumb("Orders")]
    public class OrdersModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The config app.
        /// </param>
        public OrdersModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// The on get async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnGetAsync()
        {
            // API call to get user info.
            var url = string.Format(ApiEndPoints.GetUsersEmailFilter, User.Identity!.Name);
            var users = await this.ApiClient.Get<List<UserDto>>(url);

            if (users == null || users.Count == 0){ return NotFound($"Unable to load user with ID '{User.Identity!.Name}'."); }
            await LoadAsync(users[0]);
            return Page();
        }

        /// <summary>
        /// The load async.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task LoadAsync(UserDto user)
        {
            var url = string.Format(ApiEndPoints.GetOrders, User.Identity!.Name);
            var orders = await this.ApiClient.Get<List<OrderDto>>(url);

            var ordersDict = new Dictionary<string, OrderDto>();
            var orderProductsDict = new Dictionary<string, List<OrderProductDto>>();
            if (orders is { Count: > 0 }){
                foreach (var order in orders){
                    url = string.Format(ApiEndPoints.GetOrdersProducts, order.Id);
                    var orderProduct = await this.ApiClient.Get<List<OrderProductDto>>(url);
                    ordersDict[order.Id.ToString()] = order;
                    orderProductsDict[order.Id.ToString()] = orderProduct!;
                }
            }

            url = string.Format(ApiEndPoints.GetProducts);
            var productsList = await this.ApiClient.Get<List<ProductDto>>(url);
            var productsDict = new Dictionary<string, ProductDto>();
            foreach (var productDto in productsList!){ productsDict[productDto.Id.ToString()] = productDto; }

            ViewData["OrderDict"] = ordersDict;
            ViewData["OrderProductsDict"] = orderProductsDict;
            ViewData["ProductsDict"] = productsDict;
        }
    }
}