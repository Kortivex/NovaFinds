﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderHandler.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Order Handler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.API.Handlers
{
    using Application.Services;
    using Auth;
    using CORE.Contracts;
    using CORE.Domain;
    using CORE.Mappers;
    using DTOs;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Text.Json;

    [Authorize(AuthenticationSchemes = ApiKeySchemeOptions.AuthenticateScheme)]
    public class OrderHandler(IDbContext context)
    {
        /// <summary>
        /// The order-product service.
        /// </summary>
        private readonly OrderProductService _orderProductService = new(context);

        /// <summary>
        /// The order service.
        /// </summary>
        private readonly OrderService _orderService = new(context);

        public IEnumerable<OrderDto?> GetOrders(HttpRequest request)
        {
            Logger.Debug("List Orders Handler");
            var orders = _orderService.GetAll().ToList();

            return OrderMapper.ToListDomain(orders);
        }

        public async Task<IResult> PutOrder(HttpRequest request, int orderId)
        {
            Logger.Debug("Put Order Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqOrderDto = JsonSerializer.Deserialize<OrderDto>(body);
            var orderDb = OrderMapper.ToDb(reqOrderDto!);

            var resOrder = await _orderService.GetByIdAsync(orderId);
            if (resOrder == null){ return TypedResults.BadRequest("order not found"); }

            resOrder.Status = orderDb!.Status;

            _orderService.Update(resOrder);
            await _orderService.SaveChangesAsync();

            var resultObtained = _orderService.GetAll()
                .Where(order => order.Id == resOrder.Id).ToList();
            orderDb.Id = resOrder.Id;
            orderDb.Status = resOrder.Status;
            orderDb.UserId = resOrder.UserId;
            orderDb.Date = resOrder.Date;
            orderDb.ConcurrencyStamp = resOrder.ConcurrencyStamp;

            if (resultObtained.Count == 0) return TypedResults.BadRequest("order can not updated");
            var orderDto = OrderMapper.ToDomain(orderDb);
            return TypedResults.Created($"/order/{orderDto!.Id}", orderDto);
        }

        public async Task<IResult> DeleteOrder(HttpRequest request, int id)
        {
            Logger.Debug("Delete Order Handler");
            var orders = _orderService.GetAll()
                .Where(order => order.Id == id).ToList();

            if (orders.Count == 0){ return TypedResults.NotFound(); }

            var order = orders[0];

            await _orderService.DeleteByIdAsync(order.Id);
            await _orderService.SaveChangesAsync();

            return TypedResults.Empty;
        }

        public IEnumerable<OrderProductDto?> GetOrdersProducts(HttpRequest request, int id)
        {
            Logger.Debug("List Orders-Products Handler");
            var ordersProduct = _orderProductService.GetAll()
                .Where(orderProduct => orderProduct.OrderId == id).ToList();

            return OrderProductMapper.ToListDomain(ordersProduct);
        }

        public async Task<IResult> PostOrdersProducts(HttpRequest request, int id)
        {
            Logger.Debug("Post Orders-Products Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqOrderProductDto = JsonSerializer.Deserialize<OrderProductDto>(body);
            var orderProductDb = OrderProductMapper.ToDb(reqOrderProductDto!);

            await _orderProductService.CreateAsync(orderProductDb!);
            await _orderProductService.SaveChangesAsync();

            var resultObtained = _orderProductService.GetAll()
                .Where(orderProduct => orderProduct.OrderId == id).ToList();

            if (resultObtained.Count == 0) return TypedResults.BadRequest("order-product can not created");
            var orderProductDto = OrderProductMapper.ToDomain(orderProductDb!);
            return TypedResults.Created($"/orders/{id}/products/{resultObtained[0].Id}", orderProductDto);
        }
    }
}