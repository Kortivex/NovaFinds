// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderMapper.cs" company="">
//
// </copyright>
// <summary>
//   Defines the OrderMapper converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Mappers
{
    using Domain;
    using DTOs;

    public static class OrderMapper
    {
        /// <summary>
        ///     Convert from Domain to DTO.
        /// </summary>
        public static OrderDto? ToDomain(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                Date = order.Date,
                Status = (OrderStatusType)order.Status,
                UserId = order.UserId
            };
        }

        /// <summary>
        ///     Convert from List Domain to DTO.
        /// </summary>
        public static IEnumerable<OrderDto?> ToListDomain(IEnumerable<Order> orders)
        {
            var ordersDto = new List<OrderDto?>();
            foreach (var order in orders){ ordersDto.Add(ToDomain(order)); }

            return ordersDto;
        }

        /// <summary>
        ///     Convert from Domain to DB.
        /// </summary>
        public static Order? ToDb(OrderDto order)
        {
            if ((int)order.Status > 4){ order.Status = OrderStatusType.Pending; }
            return new Order
            {
                Date = DateTime.Now,
                Status = (Enums.OrderStatusType)order.Status,
                User = null,
                OrderProducts = null
            };
        }

        /// <summary>
        ///     Convert from List Domain to DB.
        /// </summary>
        public static IEnumerable<Order?> ToListDb(IEnumerable<OrderDto> orders)
        {
            var ordersDb = new List<Order?>();
            foreach (var order in orders){ ordersDb.Add(ToDb(order)); }

            return ordersDb;
        }
    }
}