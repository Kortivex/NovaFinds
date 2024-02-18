// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterViewModel.cs" company="">
//
// </copyright>
// <summary>
//   Defines the RegisterViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Models.Areas.Identity.Account
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The register view model.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

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

        /// <summary>
        /// Gets or sets the nif.
        /// </summary>
        [Required]
        [StringLength(
            9,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 9)]
        [Display(Name = "Nif")]
        public string Nif { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required]
        [StringLength(
            100,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        [Required]
        [StringLength(
            256,
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 1)]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
    }
}