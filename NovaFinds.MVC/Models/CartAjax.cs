// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartAjaxModel.cs" company="">
//
// </copyright>
// <summary>
//   The cart ajax model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Models
{

    /// <summary>
    /// The cart ajax, ready for receive request from web with product info.
    /// </summary>
    [Serializable]
    public class CartAjaxModel
    {
        /// <summary>
        /// Gets or sets the product id.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public int Quantity { get; set; }
    }
}