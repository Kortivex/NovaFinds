// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOrderRepository.cs" company="">
//
// </copyright>
// <summary>
//   The OrderRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Contracts
{
    using Domain;
    using System.Collections;

    /// <summary>
    /// The OrderRepository interface.
    /// </summary>
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// The get all with user.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Order> GetAllWithUser();

        /// <summary>
        /// The get by order id and user id.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="id">
        /// The order id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Order> GetByUserIdAndOrderId(int userId, int id);

        /// <summary>
        /// The get by id with user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<Order> GetByIdWithUser(int id);

        /// <summary>
        /// The get by user id.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Order> GetByUserId(int userId);
        
        /// <summary>
        /// The get order status types.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable GetOrderStatusTypes();
    }
}