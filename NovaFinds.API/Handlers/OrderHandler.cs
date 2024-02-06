// --------------------------------------------------------------------------------------------------------------------
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