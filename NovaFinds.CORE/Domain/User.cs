// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="">
//
// </copyright>
// <summary>
//   Defines the User type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace NovaFinds.CORE.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;
    using Contracts;

    public sealed class User : IdentityUser<int>, IEntity
    {
        /// <summary>
        ///     Public constructor with required data
        /// </summary>
        /// <param name="nif">Real Identifier of the User</param>
        /// <param name="firstname">Name of user</param>
        /// <param name="lastname">Last Name of user</param>
        /// <param name="streetAddress">First Street Address</param>
        /// <param name="city">City</param>
        /// <param name="state">State</param>
        /// <param name="country">Country</param>
        public User(
            string nif,
            string firstname,
            string lastname,
            string streetAddress,
            string city,
            string state,
            string country)
        {
            if (string.IsNullOrEmpty(nif)) throw new ArgumentNullException(nameof(nif));
            this.Nif = nif;

            if (string.IsNullOrEmpty(firstname)) throw new ArgumentNullException(nameof(firstname));
            this.FirstName = firstname;

            if (string.IsNullOrEmpty(lastname)) throw new ArgumentNullException(nameof(lastname));
            this.LastName = lastname;

            if (string.IsNullOrEmpty(streetAddress)) throw new ArgumentNullException(nameof(streetAddress));
            this.StreetAddress = streetAddress;

            if (string.IsNullOrEmpty(city)) throw new ArgumentNullException(nameof(city));
            this.City = city;

            if (string.IsNullOrEmpty(state)) throw new ArgumentNullException(nameof(state));
            this.State = state;

            if (string.IsNullOrEmpty(country)) throw new ArgumentNullException(nameof(country));
            this.Country = country;

            this.ConcurrencyStamp = new byte[32];

            this.Carts = new HashSet<Cart>();
            this.Orders = new HashSet<Order>();
        }

        /// <summary>
        ///     Default constructor. Required because EF needs it.
        /// </summary>
        public User()
        {
            this.Nif = "";
            this.FirstName = "";
            this.LastName = "";
            this.StreetAddress = "";
            this.City = "";
            this.State = "";
            this.Country = "";
            this.ConcurrencyStamp = new byte[32];

            this.Carts = new HashSet<Cart>();
            this.Orders = new HashSet<Order>();
        }

        /// <summary>
        ///     Required, Min length = 9, Max length = 9
        ///     Real Identifier of the User
        /// </summary>
        /// <remarks>
        ///     Real Identifier of the User (DNI...)
        /// </remarks>
        [MinLength(9)]
        [StringLength(9)]
        public string Nif { get; set; }

        /// <summary>
        ///     Required, Max length = 256
        ///     Name of user
        /// </summary>
        /// <remarks>
        ///     First Name of user
        /// </remarks>
        [StringLength(256)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        ///     Required, Max length = 256
        ///     Last Name of user
        /// </summary>
        /// <remarks>
        ///     Last Name of user
        /// </remarks>
        [StringLength(256)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        ///     Required, Max length = 256
        ///     First Street Address
        /// </summary>
        /// <remarks>
        ///     Main Street Address of user
        /// </remarks>
        [StringLength(256)]
        public string StreetAddress { get; set; }

        /// <summary>
        ///     Required, Max length = 128
        ///     City
        /// </summary>
        /// <remarks>
        ///     Main City of User
        /// </remarks>
        [StringLength(128)]
        public string City { get; set; }

        /// <summary>
        ///     Required, Max length = 128
        ///     State
        /// </summary>
        /// <remarks>
        ///     Main State of User
        /// </remarks>
        [StringLength(128)]
        public string State { get; set; }

        /// <summary>
        ///     Required, Max length = 128
        ///     Country
        /// </summary>
        /// <remarks>
        ///     Main Country of User
        /// </remarks>
        [StringLength(128)]
        public string Country { get; set; }

        /// <summary>
        ///     Required
        ///     Concurrency Token
        /// </summary>
        /// <remarks>
        ///     Concurrency Control
        /// </remarks>
        public new byte[] ConcurrencyStamp { get; set; }

        public IEnumerable<Cart> Carts { get; }

        public IEnumerable<Order> Orders { get; }

        /// <summary>
        ///     Static create function (for use in LINQ queries, etc.)
        /// </summary>
        /// <param name="nif">Real Identifier of the User</param>
        /// <param name="firstname">Name of user</param>
        /// <param name="lastname">Last Name of user</param>
        /// <param name="streetAddress">First Street Address</param>
        /// <param name="city">City</param>
        /// <param name="state">State</param>
        /// <param name="country">Country</param>
        public static User Create(
            string nif,
            string firstname,
            string lastname,
            string streetAddress,
            string city,
            string state,
            string country)
        {
            return new User(nif, firstname, lastname, streetAddress, city, state, country);
        }
    }
}