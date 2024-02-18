// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserDto.cs" company="">
//
// </copyright>
// <summary>
//   Defines the UserDto dto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DTOs
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public sealed class UserDto
    {
        /// <summary>
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of the User
        /// </remarks>
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        /// <summary>
        ///     Email of the User
        /// </summary>
        /// <remarks>
        ///     Email of the user like xxx@yyy.zzz
        /// </remarks>
        [JsonPropertyName("email")]
        [EmailAddress]
        public string Email { get; set; }
        
        /// <summary>
        ///     EmailConfirmed of the User
        /// </summary>
        /// <remarks>
        ///     EmailConfirmed of the user
        /// </remarks>
        [JsonPropertyName("email_confirmed")]
        public bool EmailConfirmed { get; set; }

        /// <summary>
        ///     Phone Number of the User
        /// </summary>
        /// <remarks>
        ///     Phone Number of the user like xxxxxxxxx
        /// </remarks>
        [JsonPropertyName("phone_number")]
        [DisplayName("Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
        
        /// <summary>
        ///     PhoneNumberConfirmed of the User
        /// </summary>
        /// <remarks>
        ///     PhoneNumberConfirmed of the user
        /// </remarks>
        [JsonPropertyName("phone_number_confirmed")]
        public bool PhoneNumberConfirmed { get; set; }
        
        /// <summary>
        ///     IsActive state of the User
        /// </summary>
        /// <remarks>
        ///     IsActive state of the user
        /// </remarks>
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        /// <summary>
        ///     Username of the User
        /// </summary>
        /// <remarks>
        ///     Username of the user
        /// </remarks>
        [JsonPropertyName("username")]
        [DisplayName("Username")]
        public string UserName { get; set; }
        
        /// <summary>
        ///     Password of the User
        /// </summary>
        /// <remarks>
        ///     Password of the user
        /// </remarks>
        [JsonPropertyName("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        ///     Real Identifier of the User
        /// </summary>
        /// <remarks>
        ///     Real Identifier of the User (DNI...)
        /// </remarks>
        [JsonPropertyName("nif")]
        [MinLength(9)]
        [StringLength(9)]
        public string Nif { get; set; }

        /// <summary>
        ///     Name of user
        /// </summary>
        /// <remarks>
        ///     First Name of user
        /// </remarks>
        [JsonPropertyName("first_name")]
        [DisplayName("First Name")]
        [StringLength(256)]
        public string FirstName { get; set; }

        /// <summary>
        ///     Last Name of user
        /// </summary>
        /// <remarks>
        ///     Last Name of user
        /// </remarks>
        [JsonPropertyName("last_name")]
        [DisplayName("Last Name")]
        [StringLength(256)]
        public string LastName { get; set; }

        /// <summary>
        ///     First Street Address
        /// </summary>
        /// <remarks>
        ///     Main Street Address of user
        /// </remarks>
        [JsonPropertyName("street_address")]
        [DisplayName("Address")]
        [StringLength(256)]
        public string StreetAddress { get; set; }

        /// <summary>
        ///     City
        /// </summary>
        /// <remarks>
        ///     Main City of User
        /// </remarks>
        [JsonPropertyName("city")]
        [StringLength(128)]
        public string City { get; set; }

        /// <summary>
        ///     State
        /// </summary>
        /// <remarks>
        ///     Main State of User
        /// </remarks>
        [JsonPropertyName("state")]
        [StringLength(128)]
        public string State { get; set; }

        /// <summary>
        ///     Required, Max length = 128
        ///     Country
        /// </summary>
        /// <remarks>
        ///     Main Country of User
        /// </remarks>
        [JsonPropertyName("country")]
        [StringLength(128)]
        public string Country { get; set; }
    }
}