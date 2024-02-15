// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Details.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the DetailsModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.Order
{
    using API;
    using DTOs;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The details model for media.
    /// </summary>
    [Breadcrumb("Details")]
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The configuration.
        /// </param>
        public DetailsModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public OrderDto? Order { get; set; }
        
        /// <summary>
        /// Gets or sets the order total.
        /// </summary>
        public decimal? OrderTotal { get; set; }
        
        /// <summary>
        /// Gets or sets the order total number of items.
        /// </summary>
        public int? OrderTotalItems { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public UserDto? OrderUser { get; set; }
        
        /// <summary>
        /// Gets or sets the order-product.
        /// </summary>
        public List<OrderProductDto>? OrderProducts { get; set; }

        /// <summary>
        /// The on get async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null){ return NotFound(); }

            try{
                var url = string.Format(ApiEndPoints.GetOrder, id);
                var orderDto = await this.ApiClient.Get<OrderDto>(url);

                url = string.Format(ApiEndPoints.GetUser, orderDto!.UserId);
                var userDto = await this.ApiClient.Get<UserDto>(url);
                
                url = string.Format(ApiEndPoints.GetOrdersProducts, orderDto.Id);
                var orderProductDto = await this.ApiClient.Get<List<OrderProductDto>>(url);

                decimal totalOrder = 0;
                var totalItems = 0;
                foreach (var productDto in orderProductDto!){
                    totalOrder += productDto.Total;
                    totalItems += productDto.Quantity;
                }
                
                this.Order = orderDto;
                this.OrderTotal = totalOrder;
                this.OrderTotalItems = totalItems;
                this.OrderUser = userDto;
                this.OrderProducts = orderProductDto;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.Order == null){ return NotFound(); }

            return Page();
        }
    }
}