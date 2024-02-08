﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TpvController.cs" company="">
//
// </copyright>
// <summary>
//   Defines the TpvController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using API;
    using Application.Services;
    using CORE.Domain;
    using DAL.Context;
    using DTOs;
    using IFR.Logger;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Models;
    using SmartBreadcrumbs.Attributes;
    using Utils;
    using OrderStatusType=CORE.Enums.OrderStatusType;

    /// <summary>
    /// The tpv controller.
    /// </summary>
    [Route("Tpv")]
    public class TpvController : MainController
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
                else if (HttpContext.Session.Keys.Contains("tempUser")){ username = HttpContext.Session.GetString("tempUser"); } // EMAIL

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
                        Status = DTOs.OrderStatusType.Paid,
                        UserId = users[0].Id,
                    };

                    url = string.Format(ApiEndPoints.PostOrders, username);
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

                return BadRequest(Json(new { result = "Not items to buy!" }));
            }

            return BadRequest(Json(new { result = "User is not logged!" }));
        }
    }
}