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
    using System.Text.Json.Serialization;

    public sealed class UserDto
    {
        /// <summary>
        ///     Email of the User
        /// </summary>
        /// <remarks>
        ///     Email of the user like xxx@yyy.zzz
        /// </remarks>
        [JsonPropertyName("email")]
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
        ///     Username of the User
        /// </summary>
        /// <remarks>
        ///     Username of the user
        /// </remarks>
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        
        /// <summary>
        ///     Password of the User
        /// </summary>
        /// <remarks>
        ///     Password of the user
        /// </remarks>
        [JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        ///     Real Identifier of the User
        /// </summary>
        /// <remarks>
        ///     Real Identifier of the User (DNI...)
        /// </remarks>
        [JsonPropertyName("nif")]
        public string Nif { get; set; }

        /// <summary>
        ///     Name of user
        /// </summary>
        /// <remarks>
        ///     First Name of user
        /// </remarks>
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        ///     Last Name of user
        /// </summary>
        /// <remarks>
        ///     Last Name of user
        /// </remarks>
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        /// <summary>
        ///     First Street Address
        /// </summary>
        /// <remarks>
        ///     Main Street Address of user
        /// </remarks>
        [JsonPropertyName("street_address")]
        public string StreetAddress { get; set; }

        /// <summary>
        ///     City
        /// </summary>
        /// <remarks>
        ///     Main City of User
        /// </remarks>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        ///     State
        /// </summary>
        /// <remarks>
        ///     Main State of User
        /// </remarks>
        [JsonPropertyName("state")]
        public string State { get; set; }

        /// <summary>
        ///     Required, Max length = 128
        ///     Country
        /// </summary>
        /// <remarks>
        ///     Main Country of User
        /// </remarks>
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }
}