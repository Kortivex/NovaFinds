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
    using API;
    using CORE.Domain;
    using DTOs;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using SmartBreadcrumbs.Attributes;
    using System.Globalization;

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
            // We process the items that we have in session
            if (!HttpContext.Session.Keys.Contains("CartItems")) return View("Show");
            var productQuantity = Utils.Session.RetrieveListFromSession(HttpContext.Session, "CartItems");

            var totalWithTax = 0D;
            var totalWithoutTax = 0D;

            // Items are processed and prepared for the view
            var products = new Dictionary<ProductDto, int>();
            foreach (var product in productQuantity!){
                var productId = product["ProductId"];
                var url = string.Format(ApiEndPoints.GetProduct, productId);
                var p = await ApiClient.Get<ProductDto>(url);

                products.Add(p!, product["Quantity"]);

                totalWithTax += (p!.Price + p.Price * 21 / 100.0) * product["Quantity"];
                totalWithoutTax += p.Price * product["Quantity"];
            }

            ViewData["Products"] = products;
            ViewData["TotalTax"] = totalWithTax;
            ViewData["TotalWithOutTax"] = totalWithoutTax;

            HttpContext.Session.SetString("TotalTax", totalWithTax.ToString(CultureInfo.InvariantCulture));

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
            if (product!.Stock == 0 || !HttpContext.Session.IsAvailable) return Json(new { response = "KO" });
            var productQuantity = new List<Dictionary<string, int>>();
            // Collect session items
            if (HttpContext.Session.Keys.Contains("CartItems"))
                productQuantity = Utils.Session.RetrieveListFromSession(HttpContext.Session, "CartItems");

            // The dictionary is generated to be added later in the session
            var productQuantityExist = productQuantity!.Find(pq => pq["ProductId"] == cartAjax.ProductId);
            if (productQuantityExist != null) productQuantityExist["Quantity"] += cartAjax.Quantity;
            else
                productQuantity.Add(
                    new Dictionary<string, int>
                    {
                        { "ProductId", cartAjax.ProductId }, { "Quantity", cartAjax.Quantity }
                    });

            // The above is added to the session again
            Utils.Session.StoreListInSession(HttpContext.Session, "CartItems", productQuantity);

            if (_signInManager.IsSignedIn(User)){
                // TODO: ADD ITEM TO CART
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
            var url = string.Format(ApiEndPoints.GetProduct, cartAjax.ProductId);
            var product = await ApiClient.Get<ProductDto>(url);

            // Search and delete session
            if (product!.Stock == 0 || !HttpContext.Session.IsAvailable) return Json(new { response = "KO" });
            var productQuantity = new List<Dictionary<string, int>>();
            if (HttpContext.Session.Keys.Contains("CartItems")){
                productQuantity = Utils.Session.RetrieveListFromSession(HttpContext.Session, "CartItems");
                var productToRemove = productQuantity!.Find(x => x["ProductId"] == cartAjax.ProductId);
                productQuantity.Remove(productToRemove!);
            }

            Utils.Session.StoreListInSession(HttpContext.Session, "CartItems", productQuantity);
            
            if (_signInManager.IsSignedIn(User)){
                // TODO: DELETE ITEM FROM CART
            }

            return Json(new { response = "OK" });
        }
    }
}