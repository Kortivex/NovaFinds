// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartService.cs" company="">
//
// </copyright>
// <summary>
//   The cart service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Services
{
    using CORE.Contracts;
    using CORE.Domain;

    /// <summary>
    /// The cart service.
    /// </summary>
    public class CartService : Service<Cart>, ICartRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public CartService(IDbContext context) : base(context)
        {
        }
    }
}