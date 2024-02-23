// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TpvController.cs" company="">
//
// </copyright>
// <summary>
//   Defines the TpvController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using CORE.Domain;
    using DTOs;
    using IFR.API;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using SmartBreadcrumbs.Attributes;
    using System.Text.RegularExpressions;
    using Utils;

    /// <summary>
    /// The tpv controller.
    /// </summary>
    [Route("Tpv")]
    public partial class TpvController : MainController
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The sign in manager.
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TpvController"/> class.
        /// </summary>
        /// <param name="signInManager">
        /// The sign in manager.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public TpvController(SignInManager<User> signInManager, IConfiguration configuration) : base(configuration)
        {
            _signInManager = signInManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        [BindProperty]
        public TpvViewModel Input { get; set; }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Breadcrumb("Tpv")]
        public Task<IActionResult> Index()
        {
            Logger.Debug("Init TPV Controller");
            return Task.FromResult<IActionResult>(View("Show"));
        }

        /// <summary>
        /// The buy.
        /// </summary>
        /// <param name="buy">
        /// The buy.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost("Buy")]
        public async Task<IActionResult> Buy([FromBody] BuyAjax? buy)
        {
            Logger.Debug("Request received to Buy!");
            // It is verified that the user is logged
            if (_signInManager.IsSignedIn(User) || HttpContext.Session.Keys.Contains("tempUser")){
                var username = "";
                if (_signInManager.IsSignedIn(User)){ username = User.Identity!.Name; }
                else if (HttpContext.Session.Keys.Contains("tempUser")){
                    username = HttpContext.Session.GetString("tempUser");
                    if (buy != null){
                        if (buy.Email == ""){ return BadRequest(Json(new { result = "Email is required!" })); }
                        if (!EmailRegex().IsMatch(buy.Email)){ return BadRequest(Json(new { result = "Invalid Email format!" })); }
                        if (buy.StreetAddress == ""){ return BadRequest(Json(new { result = "Street Address is required!" })); }

                        // API call to get user info.
                        var url = string.Format(ApiEndPoints.GetUsersEmailFilter, username);
                        var users = await ApiClient.Get<List<UserDto>>(url);
                        if (users == null || users.Count == 0){ return NotFound($"Unable to load user with ID '{User.Identity!.Name}'."); }
                        var user = users[0];
                        user.Nif = "99999999X";
                        user.UserName = buy.Email;
                        user.Email = buy.Email;
                        user.StreetAddress = buy.StreetAddress;

                        // API call to update user info.
                        url = string.Format(ApiEndPoints.PutUsers, username);
                        var result = await ApiClient.Put<UserDto>(url, user);
                        if (result.Errors == null || !result.Errors.Any()){ Logger.Debug("User buying changed their email successfully."); }
                        HttpContext.Session.SetString("tempUser", buy.Email);
                        username = HttpContext.Session.GetString("tempUser");

                    }
                }

                if (buy != null){
                    // The credit card is verified
                    if (!CreditCardVerification.IsValidCardNumber(buy.CardNumber.ToString())){ return BadRequest(Json(new { result = "Credit Card Invalid!" })); }

                    // The date entered is verified
                    if (new DateTime(buy.ExpYearDate, buy.ExpMonthDate, 1) <= DateTime.Now){ return BadRequest(Json(new { result = "Credit Card Date is Invalid!" })); }

                    var url = string.Format(ApiEndPoints.GetUsersEmailFilter, username);
                    var users = await ApiClient.Get<List<UserDto>>(url);

                    if (users == null || users.Count == 0){ return NotFound($"Unable to load user with ID '{username}'."); }

                    // The new order is generated
                    var order = new OrderDto
                    {
                        Date = DateTime.Now,
                        Status = OrderStatusType.Paid,
                        UserId = users[0].Id,
                    };

                    url = string.Format(ApiEndPoints.PostUserOrders, username);
                    var orderRes = await ApiClient.Post<OrderDto>(url, order);
                    order = orderRes.Data;

                    url = string.Format(ApiEndPoints.GetCart, username);
                    var cart = await ApiClient.Get<List<CartDto>>(url);
                    if (cart!.Count != 0){
                        url = string.Format(ApiEndPoints.GetCartsItems, cart[0].Id);
                        var cartItems = await ApiClient.Get<List<CartItemDto>>(url);
                        if (cartItems != null && cartItems.Count != 0){
                            // The products are associated with the new order
                            foreach (var cartItem in cartItems){
                                url = string.Format(ApiEndPoints.GetProduct, cartItem.ProductId);
                                var product = await ApiClient.Get<ProductDto>(url);
                                var totalProduct = (decimal)((product!.Price + product.Price * 21 / 100) * cartItem.Quantity);

                                var orderProduct = new OrderProductDto
                                {
                                    Quantity = cartItem.Quantity,
                                    Total = totalProduct,
                                    ProductId = product.Id,
                                    OrderId = order!.Id
                                };

                                url = string.Format(ApiEndPoints.PostOrdersProducts, order.Id);
                                await ApiClient.Post<OrderProductDto>(url, orderProduct);

                                url = string.Format(ApiEndPoints.DeleteCartsWithStockFilter, cart[0].Id, 0);
                                ApiClient.Delete(url);
                            }

                            var shopConfig = _configuration.GetSection("Config").GetSection("General").GetSection("Shop")!;
                            var adminEmail = shopConfig.GetSection("AdminMail").Value!;
                            var shopName = shopConfig.GetSection("Url").Value!;

                            // An email is sent to the administrator
                            var emailToAdmin = new EmailDto
                            {
                                Email = adminEmail,
                                From = adminEmail,
                                Name = shopName,
                                Subject = $"Admin {adminEmail}, the Order #{order!.Id} have generated!",
                                Message = "Please go to Orders in Admin section to check the order."

                            };

                            url = ApiEndPoints.PostEmails;
                            await ApiClient.Post<EmailDto>(url, emailToAdmin);

                            // An email is sent to the user
                            var emailToUser = new EmailDto
                            {
                                Email = username!,
                                From = adminEmail,
                                Name = shopName,
                                Subject = $"Order #{order.Id} paid",
                                Message = "Please go to Orders section in your profile to check the order."

                            };

                            url = ApiEndPoints.PostEmails;
                            await ApiClient.Post<EmailDto>(url, emailToUser);

                            return Json(new { result = "OK" });

                        }
                    }

                    return BadRequest(Json(new { result = "Not items in the cart!" }));
                }

                return BadRequest(Json(new { result = "Something was wrong!" }));
            }

            return BadRequest(Json(new { result = "User is not logged!" }));
        }

        [GeneratedRegex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,4})+$")]
        private static partial Regex EmailRegex();
    }
}