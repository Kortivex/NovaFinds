// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiEndPoints.cs" company="">
//
// </copyright>
// <summary>
//   The ApiEndPoints const.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.API
{
    /// <summary>
    /// Static class with NovaFinds REST API endpoints.
    /// </summary>
    public static class ApiEndPoints
    {
        // PRODUCTS

        /// <summary>
        /// POST /products.
        /// </summary>
        public const string PostProducts = "/products";

        /// <summary>
        /// GET /products.
        /// </summary>
        public const string GetProducts = "/products";

        /// <summary>
        /// GET /products with search sort filters.
        /// </summary>
        public const string GetProductsNameSortFilters = "/products/?name={0}&size={1}";

        /// <summary>
        /// GET /products with sort filters.
        /// </summary>
        public const string GetProductsSortFilters = "/products/?size={0}&sortBy={1}";

        /// <summary>
        /// GET /products with category and sort filters.
        /// </summary>
        public const string GetProductsCategorySortFilter = "/products/?category={0}&size={1}&page={2}";

        /// <summary>
        /// GET /products with sort filters.
        /// </summary>
        public const string GetProductsSortByFilters = "/products/?size={0}&sortBy={1}&page={2}";

        /// <summary>
        /// GET /products/:id.
        /// </summary>
        public const string GetProduct = "/products/{0}";

        /// <summary>
        /// PUT /products/:id.
        /// </summary>
        public const string PutProducts = "/products/{0}";

        /// <summary>
        /// DELETE /products/:id.
        /// </summary>
        public const string DeleteProducts = "/products/{0}";

        // PRODUCT - IMAGES

        /// <summary>
        /// POST /products/:id/images.
        /// </summary>
        public const string PostProductsImages = "/products/{0}/images";

        /// <summary>
        /// GET /products/:id/images.
        /// </summary>
        public const string GetProductsImages = "/products/{0}/images";

        /// <summary>
        /// GET /products/:id/images/:image_id.
        /// </summary>
        public const string GetProductsImage = "/products/{0}/images/{1}";

        /// <summary>
        /// PUT /products/:id/images/:image_id.
        /// </summary>
        public const string PutProductsImage = "/products/{0}/images/{1}";

        /// <summary>
        /// DELETE /products/:id/images/:image_id.
        /// </summary>
        public const string DeleteProductsImage = "/products/{0}/images/{1}";
        
        /// <summary>
        /// POST /images.
        /// </summary>
        public const string PostImages = "/images";

        /// <summary>
        /// GET /images.
        /// </summary>
        public const string GetImages = "/images";

        /// <summary>
        /// GET /images with sort filters.
        /// </summary>
        public const string GetImagesSortByFilters = "/images/?size={0}&sortBy={1}&page={2}";
        
        /// <summary>
        /// GET /images/:id.
        /// </summary>
        public const string GetImage = "/images/{0}";        
        
        /// <summary>
        /// PUT /images/:id.
        /// </summary>
        public const string PutImage = "/images/{0}";
        
        /// <summary>
        /// DELETE /images/:id.
        /// </summary>
        public const string DeleteImage = "/images/{0}";

        // CATEGORIES

        /// <summary>
        /// GET /categories.
        /// </summary>
        public const string GetCategories = "/categories";

        /// <summary>
        /// GET /categories with sort filters.
        /// </summary>
        public const string GetCategoriesSortFilters = "/categories/?size={0}&page={1}";

        /// <summary>
        /// GET /categories/:id.
        /// </summary>
        public const string GetCategory = "/categories/{0}";

        // USERS

        /// <summary>
        /// POST /users.
        /// </summary>
        public const string PostUsers = "/users";

        /// <summary>
        /// PUT /users/:id.
        /// </summary>
        public const string PutUsers = "/users/{0}";

        /// <summary>
        /// GET /users.
        /// </summary>
        public const string GetUsers = "/users";

        /// <summary>
        /// GET /users/:id.
        /// </summary>
        public const string GetUser = "/users/{0}";

        /// <summary>
        /// DELETE /users/:id.
        /// </summary>
        public const string DeleteUsers = "/users/{0}";

        /// <summary>
        /// GET /users with username filter.
        /// </summary>
        public const string GetUsersUsernameFilter = "/users/?username={0}";

        /// <summary>
        /// GET /users with email filter.
        /// </summary>
        public const string GetUsersEmailFilter = "/users/?email={0}";

        // ROLES

        /// <summary>
        /// POST /roles.
        /// </summary>
        public const string PostRoles = "/roles";

        /// <summary>
        /// GET /roles.
        /// </summary>
        public const string GetRoles = "/roles";

        /// <summary>
        /// GET /roles/:id.
        /// </summary>
        public const string GetRole = "/roles/{0}";

        /// <summary>
        /// PUT /roles/:id.
        /// </summary>
        public const string PutRole = "/roles/{0}";

        /// <summary>
        /// DELETE /roles/:id.
        /// </summary>
        public const string DeleteRole = "/roles/{0}";

        // USER - ROLES

        /// <summary>
        /// GET /users/:username/roles.
        /// </summary>
        public const string GetUserRoles = "/roles/{0}/roles";

        /// <summary>
        /// PUT /users/:username/roles/:id.
        /// </summary>
        public const string PostUserRoles = "/roles/{0}/roles/{1}";

        /// <summary>
        /// DELETE /users/:username/roles/:id.
        /// </summary>
        public const string DeleteUserRoles = "/roles/{0}/roles/{1}";

        // USER - ORDERS

        /// <summary>
        /// GET /users/:username/orders.
        /// </summary>
        public const string GetUserOrders = "/users/{0}/orders";

        /// <summary>
        /// POST /users/:username/orders.
        /// </summary>
        public const string PostUserOrders = "/users/{0}/orders";

        /// <summary>
        /// PUT /users/:username/orders/:order_id.
        /// </summary>
        public const string PutUserOrders = "/users/{0}/orders/{1}";

        // ORDERS

        /// <summary>
        /// GET /orders.
        /// </summary>
        public const string GetOrders = "/orders";

        /// <summary>
        /// GET /orders/:id.
        /// </summary>
        public const string GetOrder = "/orders/{0}";

        /// <summary>
        /// GET /orders with sort filters.
        /// </summary>
        public const string GetOrdersSortFilters = "/orders/?size={0}&sortBy={1}&page={2}";

        /// <summary>
        /// PUT /orders/:id.
        /// </summary>
        public const string PutOrder = "/orders/{0}";

        /// <summary>
        /// DELETE /orders/:id.
        /// </summary>
        public const string DeleteOrder = "/orders/{0}";

        /// <summary>
        /// GET /order-types.
        /// </summary>
        public const string GetOrderTypes = "/order-types";

        /// <summary>
        /// GET /orders/:order_id/products.
        /// </summary>
        public const string GetOrdersProducts = "/orders/{0}/products";

        /// <summary>
        /// POST /orders/:order_id/products.
        /// </summary>
        public const string PostOrdersProducts = "/orders/{0}/products";

        // USER - CARTS

        /// <summary>
        /// GET /users/:username/carts.
        /// </summary>
        public const string GetCart = "/users/{0}/carts";

        // CARTS

        /// <summary>
        /// POST /carts.
        /// </summary>
        public const string PostCarts = "/carts";

        /// <summary>
        /// DELETE /carts/:cart_id.
        /// </summary>
        public const string DeleteCarts = "/carts/{0}";

        /// <summary>
        /// DELETE /carts/:cart_id. with stock filter.
        /// </summary>
        public const string DeleteCartsWithStockFilter = "/carts/{0}/?stock={1}";

        /// <summary>
        /// POST /carts/:cart_id/items.
        /// </summary>
        public const string PostCartsItems = "/carts/{0}/items";

        /// <summary>
        /// PUT /carts/:cart_id/items/:item_id.
        /// </summary>
        public const string PutCartsItems = "/carts/{0}/items/{1}";

        /// <summary>
        /// GET /carts/:cart_id/items.
        /// </summary>
        public const string GetCartsItems = "/carts/{0}/items";

        /// <summary>
        /// DELETE /carts/:cart_id/item-products/:product_id.
        /// </summary>
        public const string DeleteCartsItemProducts = "/carts/{0}/item-products/{1}";

        // EMAILS

        /// <summary>
        /// POST /emails.
        /// </summary>
        public const string PostEmails = "/emails";
    }
}