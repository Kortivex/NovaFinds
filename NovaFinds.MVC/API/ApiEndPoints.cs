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
        public const string GetProducts = "/products/?size={0}&sortBy={1}";
        
        /// <summary>
        /// GET /categories
        /// </summary>
        public const string GetCategories = "/categories";
    }
}