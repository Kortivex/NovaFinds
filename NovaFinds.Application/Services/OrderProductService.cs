// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderService.cs" company="">
//
// </copyright>
// <summary>
//   Defines the OrderService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Services
{
    using CORE.Contracts;
    using CORE.Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The order-product service.
    /// </summary>
    public class OrderProductService : Service<OrderProduct>, IOrderProductRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderProductService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public OrderProductService(IDbContext context) : base(context) {}

        /// <summary>
        /// The get by id with order product.
        /// </summary>
        /// <param name="orderId">
        /// The order id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<OrderProduct> GetByIdWithOrderProduct(int orderId)
        {
            return this.GetAll().Include(orderProduct => orderProduct.Product)
                .Include(orderProduct => orderProduct.Order)
                .Where(orderProduct => orderProduct.OrderId == orderId);
        }
    }
}