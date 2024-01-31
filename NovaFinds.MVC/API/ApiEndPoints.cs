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
        /// GET /products/:id.
        /// </summary>
        public const string GetProduct = "/products/{0}";

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
        /// GET /users with username filter.
        /// </summary>
        public const string GetUsersUsernameFilter = "/users/?username={0}";

        /// <summary>
        /// GET /users with email filter.
        /// </summary>
        public const string GetUsersEmailFilter = "/users/?email={0}";

        /// <summary>
        /// GET /users/:username/carts.
        /// </summary>
        public const string GetCart = "/users/{0}/carts";

        /// <summary>
        /// POST /carts.
        /// </summary>
        public const string PostCarts = "/carts";
        
        /// <summary>
        /// DELETE /carts/:cart_id.
        /// </summary>
        public const string DeleteCarts = "/carts/{0}";

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
    }
}