// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderProduct.cs" company="">
//
// </copyright>
// <summary>
//   Defines the OrderProduct type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Domain
{
    using Contracts;

    /// <summary>
    ///     Order-Product table
    /// </summary>
    /// <remarks>
    ///     Table of the Order-Product
    /// </remarks>
    public sealed class OrderProduct : IEntity
    {
        /// <summary>
        ///     Public constructor with required data
        /// </summary>
        /// <param name="productId">External Product Ref.</param>
        /// <param name="orderId">External Order Ref.</param>
        /// <param name="quantity">Amount of product</param>
        /// <param name="total">Amount of order</param>
        public OrderProduct(int productId, int orderId, int quantity = 0, decimal total = 0)
        {
            this.Quantity = quantity;
            this.Total = total;
            this.ProductId = productId;
            this.OrderId = orderId;
            this.ConcurrencyStamp = new byte[32];

            this.Product = new Product();
            this.Order = new Order();
        }

        /// <summary>
        ///     Default constructor. Present because EF needs it.
        /// </summary>
        private OrderProduct()
        {
            this.Quantity = 0;
            this.Total = 0;
            this.ConcurrencyStamp = new byte[32];

            this.Product = new Product();
            this.Order = new Order();
        }

        /// <summary>
        ///     Identity, Indexed, Required
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Order-Product
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        ///     Required, Default value = 0
        ///     Amount of product
        /// </summary>
        /// <remarks>
        ///     Total product requested in one order
        /// </remarks>
        public int Quantity { get; set; }

        /// <summary>
        ///     Required, Default value = 0
        ///     Amount of order
        /// </summary>
        /// <remarks>
        ///     Total amount of one product in one order
        /// </remarks>
        public decimal Total { get; set; }

        /// <summary>
        ///     Required
        ///     External Product Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related Product
        /// </remarks>
        public int ProductId { get; set; }

        /// <summary>
        ///     Required
        ///     External Order Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related Order
        /// </remarks>
        public int OrderId { get; set; }

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
        ///     Relation Product-Order & Product
        /// </remarks>
        public Product? Product { get; set; }

        /// <summary>
        ///     Required
        /// </summary>
        /// <remarks>
        ///     Relation Product-Order & Order
        /// </remarks>
        public Order? Order { get; set; }

        /// <summary>
        ///     Static create function (for use in LINQ queries, etc.)
        /// </summary>
        /// <param name="productId">External Product Ref.</param>
        /// <param name="orderId">External Order Ref.</param>
        /// <param name="quantity">Amount of product</param>
        /// <param name="total">Amount of order</param>
        public static OrderProduct Create(int productId, int orderId, int quantity = 0, decimal total = 0)
        {
            return new OrderProduct(productId, orderId, quantity, total);
        }
    }
}