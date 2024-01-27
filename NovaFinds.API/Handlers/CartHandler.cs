// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartHandler.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Cart Handler type.
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
    using System.Text.Json;

    [Authorize(AuthenticationSchemes = ApiKeySchemeOptions.AuthenticateScheme)]
    public class CartHandler(IDbContext context, UserManager<User> userManager)
    {
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService _userService = new(context, userManager);

        /// <summary>
        /// The cart service.
        /// </summary>
        private readonly CartService _cartService = new(context);

        /// <summary>
        /// The cart-item service.
        /// </summary>
        private readonly CartItemService _cartItemService = new(context);

        public async Task<IResult> PostCarts(HttpRequest request)
        {
            Logger.Debug("Post Cart Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqCartDto = JsonSerializer.Deserialize<CartDto>(body);
            var username = reqCartDto!.UserName;
            var cartDb = CartMapper.ToDb(reqCartDto);

            var users = _userService.GetAll()
                .Where(user => user.UserName == username).ToList();

            if (users.Count == 0) return TypedResults.BadRequest("username not exist");
            {
                var user = users[0];
                cartDb!.UserId = user.Id;
                await _cartService.CreateAsync(cartDb);
                await _cartService.SaveChangesAsync();

                var resultObtained = _cartService.GetAll()
                    .Where(cart => cart.UserId == user.Id).ToList();

                if (resultObtained.Count == 0) return TypedResults.BadRequest("cart can not created");
                var cartDto = CartMapper.ToDomain(cartDb);
                cartDto!.UserId = user.Id;
                cartDto.UserName = username;
                return TypedResults.Created($"/carts/{resultObtained[0].Id}", cartDto);
            }
        }

        public IEnumerable<CartItemDto?> GetCartItems(HttpRequest request, int cartId)
        {
            Logger.Debug("List Cart-Items Handler");
            var cartItems = _cartItemService.GetAll()
                .Where(cartItem => cartItem.CartId == cartId).ToList();
            return CartItemMapper.ToListDomain(cartItems);
        }

        public async Task<IResult> PostCartItems(HttpRequest request, int cartId)
        {
            Logger.Debug("Post Cart-Items Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqCartItemDto = JsonSerializer.Deserialize<CartItemDto>(body);
            var cartItemDb = CartItemMapper.ToDb(reqCartItemDto!);
            cartItemDb!.CartId = cartId;

            await _cartItemService.CreateAsync(cartItemDb);
            await _cartItemService.SaveChangesAsync();

            var resultObtained = _cartItemService.GetAll()
                .Where(cartItem => cartItem.CartId == cartId).ToList();

            if (resultObtained.Count == 0) return TypedResults.BadRequest("cart-item can not created");
            var cartItemDto = CartItemMapper.ToDomain(cartItemDb);
            return TypedResults.Created($"/carts/{cartId}/items/{resultObtained[0].Id}", cartItemDto);
        }

        public async Task<IResult> PutCartItems(HttpRequest request, int cartId, int itemId)
        {
            Logger.Debug("Put Cart-Items Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqCartItemDto = JsonSerializer.Deserialize<CartItemDto>(body);
            var cartItemDb = CartItemMapper.ToDb(reqCartItemDto!);
            cartItemDb!.Id = itemId;

            var resCartItem = await _cartItemService.GetByIdAsync(itemId);
            resCartItem!.Quantity = cartItemDb.Quantity;

            _cartItemService.Update(resCartItem);
            await _cartItemService.SaveChangesAsync();

            var resultObtained = _cartItemService.GetAll()
                .Where(cartItem => cartItem.Id == cartItemDb.Id).ToList();

            if (resultObtained.Count == 0) return TypedResults.BadRequest("cart-item can not updated");
            var cartItemDto = CartItemMapper.ToDomain(cartItemDb);
            return TypedResults.Created($"/carts/{cartId}/items/{resultObtained[0].Id}", cartItemDto);
        }
    }
}