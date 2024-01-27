// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartItem.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CartItem type.
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
    public sealed class CartItem : IEntity
    {
        /// <summary>
        ///     Public constructor with required data
        /// </summary>
        /// <param name="cartId">External Cart Ref.</param>
        /// <param name="productId">External Product Ref.</param>
        /// <param name="quantity">Cart Item Quantity</param>
        public CartItem(int cartId, int productId, int quantity = 0)
        {
            this.CartId = cartId;
            this.ProductId = productId;
            this.Quantity = quantity;
            this.ConcurrencyStamp = new byte[32];

            this.Cart = new Cart();
            this.Product = new Product();
        }

        /// <summary>
        ///     Default constructor. Present because EF needs it.
        /// </summary>
        public CartItem()
        {
            this.Quantity = 0;
            this.ConcurrencyStamp = new byte[32];

            this.Cart = new Cart();
            this.Product = new Product();
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
        ///     Required, Default value = 0
        ///     Amount of product
        /// </summary>
        /// <remarks>
        ///     Total product requested
        /// </remarks>
        public int Quantity { get; set; }

        /// <summary>
        ///     Required
        ///     External Cart Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related Cart
        /// </remarks>
        public int CartId { get; set; }

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
        ///     Relation CartItem & Cart
        /// </remarks>
        public Cart? Cart { get; set; }

        /// <summary>
        ///     Required
        /// </summary>
        /// <remarks>
        ///     Relation CartItem & Product
        /// </remarks>
        public Product? Product { get; set; }

        /// <summary>
        ///     Static create function (for use in LINQ queries, etc.)
        /// </summary>
        /// <param name="cartId">External Cart Ref.</param>
        /// <param name="productId">External Product Ref.</param>
        /// <param name="quantity">Cart Item Quantity</param>
        public static CartItem Create(int cartId, int productId, int quantity = 0)
        {
            return new CartItem(cartId, productId, quantity);
        }
    }
}