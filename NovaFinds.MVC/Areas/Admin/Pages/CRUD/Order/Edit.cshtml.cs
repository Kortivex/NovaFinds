// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Edit.cshtml.cs" company="">
//
// </copyright>
// <summary>
//   Defines the EditModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD.Order
{
    using DTOs;
    using IFR.API;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The edit model for media.
    /// </summary>
    [Breadcrumb("Edit")]
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        /// <summary>
        /// The API client.
        /// </summary>
        protected ApiClient ApiClient { get; private set; }

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The configuration.
        /// </param>
        public EditModel(IConfiguration config)
        {
            this.ApiClient = new ApiClient(config);
            _configuration = config;
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        [BindProperty]
        public OrderDto? Order { get; set; }

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
            if (id == null) return NotFound();

            string url;
            try{
                url = string.Format(ApiEndPoints.GetOrder, id);
                var orderDto = await this.ApiClient.Get<OrderDto>(url);

                this.Order = orderDto!;
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            if (this.Order == null) return NotFound();

            url = string.Format(ApiEndPoints.GetOrderTypes);
            var orderTypes = await this.ApiClient.Get<List<OrderStatusDto>>(url);

            ViewData["OrderStatusType"] = new SelectList(orderTypes, "Id", "Name");

            return Page();
        }

        /// <summary>
        /// The on post async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid){
                var url = string.Format(ApiEndPoints.GetOrderTypes);
                var orderTypes = await this.ApiClient.Get<List<OrderStatusDto>>(url);

                ViewData["OrderStatusType"] = new SelectList(orderTypes, "Id", "Name");

                return Page();
            }

            try{
                var url = string.Format(ApiEndPoints.GetOrder, this.Order!.Id);
                var orderDto = await this.ApiClient.Get<OrderDto>(url);

                if (orderDto != null){
                    orderDto.Status = this.Order.Status;
                    if (this.Order.Status == OrderStatusType.Shipped) orderDto.Date = DateTime.Now;

                    url = string.Format(ApiEndPoints.PutOrder, this.Order!.Id);
                    var orderResult = await this.ApiClient.Put<OrderDto>(url, orderDto);

                    if (orderResult.Errors != null && orderResult.Errors.Count() >= 0){ return BadRequest(); }

                    url = string.Format(ApiEndPoints.GetUser, orderDto.UserId);
                    var userDto = await this.ApiClient.Get<UserDto>(url);

                    var shopConfig = _configuration.GetSection("Config").GetSection("General").GetSection("Shop")!;
                    var adminEmail = shopConfig.GetSection("AdminMail").Value!;
                    var shopName = shopConfig.GetSection("Url").Value!;

                    // An email is sent to the user
                    var emailToUser = new EmailDto
                    {
                        Email = userDto!.Email,
                        From = adminEmail,
                        Name = shopName,
                        Subject = $"Order #{orderDto.Id} have change the status!",
                        Message = "Please go to Orders section in your profile to check the order."
                    };

                    url = ApiEndPoints.PostEmails;
                    await this.ApiClient.Post<EmailDto>(url, emailToUser);
                }
            }
            catch (Exception exception){
                Logger.Error(exception.Message);
                return RedirectToPage("~/");
            }

            return RedirectToPage("./Index");
        }
    }
}