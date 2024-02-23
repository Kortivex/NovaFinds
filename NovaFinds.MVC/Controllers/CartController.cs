// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartController.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CartController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Controllers
{
    using CORE.Domain;
    using DTOs;
    using IFR.API;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using SmartBreadcrumbs.Attributes;

    /// <summary>
    /// The cart controller.
    /// </summary>
    [Route("Cart")]
    public class CartController : MainController
    {
        /// <summary>
        /// The sign in manager.
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="signInManager">
        /// The sign in manager.
        /// </param>
        public CartController(IConfiguration configuration, SignInManager<User> signInManager) : base(configuration)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// The index to show the entire cart.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Breadcrumb("Cart")]
        public async Task<IActionResult> Index()
        {
            var username = "";
            if (_signInManager.IsSignedIn(User)){ username = User.Identity!.Name; }
            else if (HttpContext.Session.Keys.Contains("tempUser")){ username = HttpContext.Session.GetString("tempUser"); } // EMAIL 

            var url = string.Format(ApiEndPoints.GetUsersEmailFilter, username);
            var users = await ApiClient.Get<List<UserDto>>(url);
            if (users != null && users.Count != 0){
                var user = users[0];

                url = string.Format(ApiEndPoints.GetCart, user.UserName);
                var carts = await ApiClient.Get<List<CartDto>>(url);
                if (carts != null && carts.Count != 0){
                    var cart = carts[0];
                    url = string.Format(ApiEndPoints.GetCartsItems, cart.Id);
                    var cartItems = await ApiClient.Get<List<CartItemDto>>(url);
                    if (cartItems != null && cartItems.Count != 0){
                        var totalWithTax = 0D;
                        var totalWithoutTax = 0D;
                        var products = new Dictionary<ProductDto, int>();
                        foreach (var cartItem in cartItems){
                            url = string.Format(ApiEndPoints.GetProduct, cartItem.ProductId);
                            var product = await ApiClient.Get<ProductDto>(url);
                            products.Add(product!, cartItem.Quantity);
                            totalWithTax += (product!.Price + product.Price * 21 / 100.0) * cartItem.Quantity;
                            totalWithoutTax += product.Price * cartItem.Quantity;
                        }
                        ViewData["Products"] = products;
                        ViewData["TotalTax"] = totalWithTax;
                        ViewData["TotalWithOutTax"] = totalWithoutTax;
                    }
                }
            }
            return View("Show");
        }

        /// <summary>
        /// The add item method.
        /// </summary>
        /// <param name="cartAjax">
        /// The cart ajax.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost("AddItem")]
        public async Task<ActionResult<CartAjaxModel>> AddItem([FromBody] CartAjaxModel cartAjax)
        {
            // Get the item to verify
            var url = string.Format(ApiEndPoints.GetProduct, cartAjax.ProductId);
            var product = await ApiClient.Get<ProductDto>(url);

            // It is verified that the units are not under minimum
            if (!(product!.Stock >= cartAjax.Quantity)) return Json(new { response = "KO" });

            var username = "";
            if (_signInManager.IsSignedIn(User)){ username = User.Identity!.Name; }
            else if (HttpContext.Session.Keys.Contains("tempUser")){ username = HttpContext.Session.GetString("tempUser"); } // EMAIL 

            // API call to get user info.
            url = string.Format(ApiEndPoints.GetUsersEmailFilter, username);
            var users = await ApiClient.Get<List<UserDto>>(url);
            if (users != null && users.Count != 0){
                var user = users[0];

                url = string.Format(ApiEndPoints.GetCart, user.UserName);
                var carts = await ApiClient.Get<List<CartDto>>(url);
                if (carts == null || carts.Count == 0){
                    var newCart = new CartDto
                    {
                        UserName = username!,
                        Date = DateTime.Now,
                        UserId = user.Id
                    };
                    url = string.Format(ApiEndPoints.PostCarts);
                    var result = await ApiClient.Post<CartDto>(url, newCart);
                    if (result.Errors != null){ return Json(new { response = "KO" }); }

                    url = string.Format(ApiEndPoints.GetCart, user.UserName);
                    carts = await ApiClient.Get<List<CartDto>>(url);
                }
                var cart = carts![0];
                url = string.Format(ApiEndPoints.GetCartsItems, cart.Id);
                var cartItems = await ApiClient.Get<List<CartItemDto>>(url);
                if (cartItems != null && cartItems.Count != 0){
                    var itemInCart = false;
                    foreach (var cartItem in cartItems){
                        cartItem.Quantity = cartAjax.Quantity;
                        if (cartItem.ProductId == cartAjax.ProductId){
                            itemInCart = true;
                            url = string.Format(ApiEndPoints.PutCartsItems, cart.Id, cartItem.Id);
                            var result = await ApiClient.Put<CartItemDto>(url, cartItem);
                            if (result.Errors != null){ return Json(new { response = "KO" }); }
                        }
                    }
                    if (!itemInCart){
                        url = string.Format(ApiEndPoints.PostCartsItems, cart.Id);
                        var newCartItem = new CartItemDto
                        {
                            CartId = cart.Id,
                            ProductId = cartAjax.ProductId,
                            Quantity = cartAjax.Quantity
                        };
                        var result = await ApiClient.Post<CartItemDto>(url, newCartItem);
                        if (result.Errors != null){ return Json(new { response = "KO" }); }
                    }
                }
                else{
                    url = string.Format(ApiEndPoints.PostCartsItems, cart.Id);
                    var newCartItem = new CartItemDto
                    {
                        CartId = cart.Id,
                        ProductId = cartAjax.ProductId,
                        Quantity = cartAjax.Quantity
                    };
                    await ApiClient.Post<CartItemDto>(url, newCartItem);
                }
            }

            return Json(new { response = "OK" });
        }

        /// <summary>
        /// The remove item.
        /// </summary>
        /// <param name="cartAjax">
        /// The cart ajax.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost("RemoveItem")]
        public async Task<ActionResult<CartAjaxModel>> RemoveItem([FromBody] CartAjaxModel cartAjax)
        {
            var username = "";
            if (_signInManager.IsSignedIn(User)){ username = User.Identity!.Name; }
            else if (HttpContext.Session.Keys.Contains("tempUser")){ username = HttpContext.Session.GetString("tempUser"); } // EMAIL

            var url = string.Format(ApiEndPoints.GetCart, username);
            var carts = await ApiClient.Get<List<CartDto>>(url);
            if (carts != null || carts!.Count != 0){
                var cart = carts[0];
                url = string.Format(ApiEndPoints.DeleteCartsItemProducts, cart.Id, cartAjax.ProductId);
                ApiClient.Delete(url);
            }

            return Json(new { response = "OK" });
        }
    }
}