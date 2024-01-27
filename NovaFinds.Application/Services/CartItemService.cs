// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartItemService.cs" company="">
//
// </copyright>
// <summary>
//   The cart-item service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Services
{
    using CORE.Contracts;
    using CORE.Domain;

    /// <summary>
    /// The cart service.
    /// </summary>
    public class CartItemService : Service<CartItem>, ICartItemRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public CartItemService(IDbContext context) : base(context)
        {
        }
    }
}