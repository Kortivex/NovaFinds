// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderProductMapper.cs" company="">
//
// </copyright>
// <summary>
//   Defines the OrderProductMapper converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Mappers
{
    using Domain;
    using DTOs;

    public static class OrderProductMapper
    {
        /// <summary>
        ///     Convert from Domain to DTO.
        /// </summary>
        public static OrderProductDto? ToDomain(OrderProduct orderProduct)
        {
            return new OrderProductDto
            {
                Id = orderProduct.Id,
                Quantity = orderProduct.Quantity,
                Total = orderProduct.Total,
                OrderId = orderProduct.OrderId,
                ProductId = orderProduct.ProductId
            };
        }

        /// <summary>
        ///     Convert from List Domain to DTO.
        /// </summary>
        public static IEnumerable<OrderProductDto?> ToListDomain(IEnumerable<OrderProduct> orderProducts)
        {
            var orderProductsDto = new List<OrderProductDto?>();
            foreach (var orderProduct in orderProducts){ orderProductsDto.Add(ToDomain(orderProduct)); }

            return orderProductsDto;
        }

        /// <summary>
        ///     Convert from Domain to DB.
        /// </summary>
        public static OrderProduct? ToDb(OrderProductDto orderProduct)
        {
            return new OrderProduct(orderProduct.ProductId, orderProduct.OrderId, orderProduct.Quantity, orderProduct.Total)
            {
                Quantity = orderProduct.Quantity,
                Total = orderProduct.Total,
                OrderId = orderProduct.OrderId,
                ProductId = orderProduct.ProductId,
                Order = null,
                Product = null
            };
        }

        /// <summary>
        ///     Convert from List Domain to DB.
        /// </summary>
        public static IEnumerable<OrderProduct?> ToListDb(IEnumerable<OrderProductDto> orderProducts)
        {
            var orderProductsDb = new List<OrderProduct?>();
            foreach (var orderProduct in orderProducts){ orderProductsDb.Add(ToDb(orderProduct)); }

            return orderProductsDb;
        }
    }
}