// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuyAjax.cs" company="">
//
// </copyright>
// <summary>
//   The buy ajax.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Models
{

    /// <summary>
    /// The buy ajax object, ready for receive request from web with credit card info.
    /// </summary>
    [Serializable]
    public class BuyAjax
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        public string StreetAddress { get; set; }
        
        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        public long CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the exp month date.
        /// </summary>
        public int ExpMonthDate { get; set; }

        /// <summary>
        /// Gets or sets the exp year date.
        /// </summary>
        public int ExpYearDate { get; set; }
    }
}