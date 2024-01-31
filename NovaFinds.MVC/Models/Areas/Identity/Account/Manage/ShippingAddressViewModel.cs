// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShippingAddressViewModel.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ShippingAddressViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Models.Areas.Identity.Account.Manage
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The shipping address view model.
    /// </summary>
    public class ShippingAddressViewModel
    {
        /// <summary>
        /// Gets or sets the shipping city.
        /// </summary>
        [Required]
        [Display(Name = "Shipping City")]
        public string ShippingCity { get; set; }

        /// <summary>
        /// Gets or sets the shipping country.
        /// </summary>
        [Required]
        [Display(Name = "Shipping Country")]
        public string ShippingCountry { get; set; }

        [Required]
        [Display(Name = "Shipping State")]
        public string ShippingState { get; set; }

        /// <summary>
        /// Gets or sets the shipping street address.
        /// </summary>
        [Required]
        [StringLength(
            256,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 1)]
        [Display(Name = "Shipping Street Address")]
        public string ShippingStreetAddress { get; set; }
    }
}