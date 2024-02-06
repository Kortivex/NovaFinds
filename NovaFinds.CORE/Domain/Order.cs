// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Order.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Order type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Domain
{
    using Contracts;
    using Enums;

    /// <summary>
    ///     Order table
    /// </summary>
    /// <remarks>
    ///     Table of the Order
    /// </remarks>
    public sealed class Order : IEntity
    {
        /// <summary>
        ///     Public constructor with required data
        /// </summary>
        /// <param name="userId">External User Ref.</param>
        /// <param name="status">Status of the Order</param>
        /// <param name="date">Date</param>
        public Order(
            int userId,
            OrderStatusType status = OrderStatusType.Pending,
            DateTime date = default)
        {
            this.Date = date;
            this.Status = status;
            this.UserId = userId;
            this.ConcurrencyStamp = new byte[32];

            this.User = new User();
            this.OrderProducts = new HashSet<OrderProduct>();
        }

        /// <summary>
        ///     Default constructor. Present because EF needs it.
        /// </summary>
        public Order()
        {
            this.Date = DateTime.Now;
            this.Status = OrderStatusType.Pending;
            this.ConcurrencyStamp = new byte[32];

            this.User = new User();
            this.OrderProducts = new HashSet<OrderProduct>();
        }

        /// <summary>
        ///     Identity, Indexed, Required
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Order
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        ///     Required, Default value = DateTime.Now
        ///     Date
        /// </summary>
        /// <remarks>
        ///     Date when the row was created
        /// </remarks>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Required, Default value = OrderStatusType.Pending
        ///     Status of the Order
        /// </summary>
        /// <remarks>
        ///     Status of the Order by OrderStatusType
        /// </remarks>
        public OrderStatusType Status { get; set; }

        /// <summary>
        ///     Required
        ///     Id of the User
        /// </summary>
        /// <remarks>
        ///     Id assigned to User
        /// </remarks>
        public int UserId { get; set; }

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
        ///     Relation Oder & User
        /// </remarks>
        public User? User { get; set; }

        public IEnumerable<OrderProduct>? OrderProducts { get; set; }

        /// <summary>
        ///     Static create function (for use in LINQ queries, etc.)
        /// </summary>
        /// <param name="userId">External User Ref.</param>
        /// <param name="status">Status of the Order</param>
        /// <param name="date">Date of the Order</param>
        public static Order Create(
            int userId,
            OrderStatusType status = OrderStatusType.Pending,
            DateTime date = default)
        {
            return new Order(userId, status, date);
        }
    }
}