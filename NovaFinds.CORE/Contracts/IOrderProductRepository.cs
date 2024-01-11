// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrderProductRepository.cs" company="">
//
// </copyright>
// <summary>
//   Defines the IOrderProductRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Contracts
{
    using Domain;

    /// <summary>
    /// The OrderProductRepository interface.
    /// </summary>
    public interface IOrderProductRepository : IRepository<OrderProduct>
    {
        /// <summary>
        /// The get by id with order product.
        /// </summary>
        /// <param name="orderId">
        /// The order id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<OrderProduct> GetByIdWithOrderProduct(int orderId);
    }
}