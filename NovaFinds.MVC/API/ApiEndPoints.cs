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
        /// GET /products
        /// </summary>
        public const string GetProducts = "/products";
        
        /// <summary>
        /// GET /products with sort filters.
        /// </summary>
        public const string GetProductsSortFilters = "/products/?size={0}&sortBy={1}";
        
        /// <summary>
        /// GET /products with category and sort filters.
        /// </summary>
        public const string GetProductsCategorySortFilter = "/products/?category={0}&size={1}&page={2}";
        
        /// <summary>
        /// GET /products/:id
        /// </summary>
        public const string GetProduct = "/products/{0}";
        
        /// <summary>
        /// GET /categories
        /// </summary>
        public const string GetCategories = "/categories";
        
        /// <summary>
        /// GET /categories with sort filters.
        /// </summary>
        public const string GetCategoriesSortFilters = "/categories/?size={0}&page={1}";
        
        /// <summary>
        /// GET /categories/:id
        /// </summary>
        public const string GetCategory = "/categories/{0}";
    }
}