// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cart.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Cart type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Domain
{

    using Contracts;

    /// <summary>
    ///     Shopping Cart table
    /// </summary>
    /// <remarks>
    ///     Table of the Shopping Cart
    /// </remarks>
    public sealed class Cart : IEntity
    {
        /// <summary>
        ///     Public constructor with required data
        /// </summary>
        /// <param name="userId">External User Ref.</param>
        /// <param name="date">Cart Creation Date</param>
        public Cart(int userId, DateTime date = default)
        {
            this.UserId = userId;
            this.Date = date;
            this.ConcurrencyStamp = new byte[32];

            this.User = new User();
            this.CartItems = new List<CartItem>();
        }

        /// <summary>
        ///     Default constructor. Present because EF needs it.
        /// </summary>
        public Cart()
        {
            this.Date = DateTime.Now;
            this.ConcurrencyStamp = new byte[32];

            this.User = new User();
            this.CartItems = new List<CartItem>();
        }

        /// <summary>
        ///     Identity, Indexed, Required
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Shopping Cart
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        ///     Required
        ///     External User Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related User
        /// </remarks>
        public int UserId { get; set; }

        /// <summary>
        ///     Default value = DateTime.Now
        ///     Initial Date of Shopping Cart
        /// </summary>
        /// <remarks>
        ///     Initial Date of Shopping Cart when User initialize session
        /// </remarks>
        public DateTime? Date { get; set; }

        /// <summary>
        ///     Required
        ///     Concurrency Token
        /// </summary>
        /// <remarks>
        ///     Concurrency Control
        /// </remarks>
        public byte[] ConcurrencyStamp { get; set; }

        /// <summary>
        ///     Required
        /// </summary>
        /// <remarks>
        ///     Relation Cart & User
        /// </remarks>
        public User User { get; set; }

        /// <summary>
        ///     Required
        /// </summary>
        /// <remarks>
        ///     Relation Cart & CartItems
        /// </remarks>
        public IEnumerable<CartItem> CartItems { get; }

        /// <summary>
        ///     Static create function (for use in LINQ queries, etc.)
        /// </summary>
        /// <param name="userId">External User Ref.</param>
        /// <param name="date">Cart Creation Date</param>
        public static Cart Create(int userId, DateTime date = default)
        {
            return new Cart(userId, date);
        }
    }
}