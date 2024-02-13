// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartItemMapper.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CartItemMapper converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Mappers
{
    using CORE.Domain;
    using DTOs;

    public static class CartItemMapper
    {
        /// <summary>
        ///     Convert from Domain to DTO.
        /// </summary>
        public static CartItemDto? ToDomain(CartItem cartItem)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };
        }

        /// <summary>
        ///     Convert from List Domain to DTO.
        /// </summary>
        public static IEnumerable<CartItemDto?> ToListDomain(IEnumerable<CartItem> cartItems)
        {
            var cartsItemsDto = new List<CartItemDto?>();
            foreach (var cartItem in cartItems){ cartsItemsDto.Add(ToDomain(cartItem)); }

            return cartsItemsDto;
        }

        /// <summary>
        ///     Convert from Domain to DB.
        /// </summary>
        public static CartItem? ToDb(CartItemDto cartItem)
        {
            return new CartItem
            {
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Cart = null,
                Product = null
            };
        }

        /// <summary>
        ///     Convert from List Domain to DB.
        /// </summary>
        public static IEnumerable<CartItem?> ToListDb(IEnumerable<CartItemDto> cartsItems)
        {
            var cartItemsDb = new List<CartItem?>();
            foreach (var cartItem in cartsItems){ cartItemsDb.Add(ToDb(cartItem)); }

            return cartItemsDb;
        }
    }
}