// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartMapper.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CartMapper converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Mappers
{
    using CORE.Domain;
    using DTOs;

    public static class CartMapper
    {
        /// <summary>
        ///     Convert from Domain to DTO.
        /// </summary>
        public static CartDto? ToDomain(Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                UserName = cart.UserName,
                Date = cart.Date
            };
        }

        /// <summary>
        ///     Convert from List Domain to DTO.
        /// </summary>
        public static IEnumerable<CartDto?> ToListDomain(IEnumerable<Cart> carts)
        {
            var cartsDto = new List<CartDto?>();
            foreach (var cart in carts){ cartsDto.Add(ToDomain(cart)); }

            return cartsDto;
        }

        /// <summary>
        ///     Convert from Domain to DB.
        /// </summary>
        public static Cart? ToDb(CartDto cart)
        {
            return new Cart
            {
                UserId = cart.UserId,
                Date = DateTime.Now,
                User = null,
                CartItems = null
            };
        }

        /// <summary>
        ///     Convert from List Domain to DB.
        /// </summary>
        public static IEnumerable<Cart?> ToListDb(IEnumerable<CartDto> carts)
        {
            var cartsDb = new List<Cart?>();
            foreach (var cart in carts){ cartsDb.Add(ToDb(cart)); }

            return cartsDb;
        }
    }
}