// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TpvViewModel.cs" company="">
//
// </copyright>
// <summary>
//   Defines the TpvViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The tpv view model.
    /// </summary>
    public class TpvViewModel
    {
        
        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        [Required]
        [StringLength(
            60,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 5)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        [Required]
        [StringLength(
            16,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 16)]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the exp month date.
        /// </summary>
        [Required]
        [StringLength(
            2,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 1)]
        [Display(Name = "Exp. Month Date")]
        public string ExpMonthDate { get; set; }

        /// <summary>
        /// Gets or sets the exp year date.
        /// </summary>
        [Required]
        [StringLength(
            4,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 4)]
        [Display(Name = "Exp. Year Date")]
        public string ExpYearDate { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required]
        [StringLength(
            256,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 1)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Required]
        [StringLength(
            256,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}