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
    using CORE.Enums;
    using Microsoft.EntityFrameworkCore;
    using System.Collections;

    /// <summary>
    /// The order service.
    /// </summary>
    public class OrderService : Service<Order>, IOrderRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public OrderService(IDbContext context) : base(context) {}

        /// <summary>
        /// The get all with user.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Order> GetAllWithUser()
        {
            return this.GetAll().Include(order => order.User);
        }

        /// <summary>
        /// The get by user id and order id.
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
        public IQueryable<Order> GetByUserIdAndOrderId(int userId, int id)
        {
            return this.GetAll().Where(order => order.UserId == userId).Where(order => order.Id == id);
        }

        /// <summary>
        /// The get by id with user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Order> GetByIdWithUser(int id)
        {
            return this.GetAll().Include(order => order.User).Where(order => order.Id == id);
        }

        /// <summary>
        /// The get by user id.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Order> GetByUserId(int userId)
        {
            return this.GetAll().Where(order => order.UserId == userId);
        }

        /// <summary>
        /// The get order status types.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable GetOrderStatusTypes()
        {
            return from OrderStatusType t in Enum.GetValues(typeof(OrderStatusType))
                select new { Id = (int)t, Name = t.ToString() };
        }
    }
}