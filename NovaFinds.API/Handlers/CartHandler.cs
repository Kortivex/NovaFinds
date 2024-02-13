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
    using Application.Mappers;
    using Application.Services;
    using Auth;
    using CORE.Contracts;
    using CORE.Domain;
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

        /// <summary>
        /// The product service.
        /// </summary>
        private readonly ProductService _productService = new(context);

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

        public async Task<IResult> DeleteCart(HttpRequest request, int cartId)
        {
            Logger.Debug("Delete Cart Handler");

            var cartItems = _cartItemService.GetAll()
                .Where(cartItem => cartItem.CartId == cartId)
                .ToList();

            var stock = "1";
            if (request.Query.ContainsKey("stock")){ stock = request.Query["stock"].ToString(); }

            if (stock == "1"){
                foreach (var item in cartItems){
                    var product = await _productService.GetByIdAsync(item.ProductId);
                    product!.Stock += item.Quantity;
                    await _productService.SaveChangesAsync();
                }
            }

            await _cartService.DeleteByIdAsync(cartId);
            await _cartService.SaveChangesAsync();

            return TypedResults.Empty;
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

            var product = await _productService.GetByIdAsync(cartItemDb.ProductId);
            if (product!.Stock >= cartItemDb.Quantity){
                product.Stock -= cartItemDb.Quantity;
                await _productService.SaveChangesAsync();
            }
            else{ return TypedResults.BadRequest("cart-item can not updated"); }

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
            resCartItem!.Quantity += cartItemDb.Quantity;

            var product = await _productService.GetByIdAsync(cartItemDb.ProductId);
            if (product!.Stock >= cartItemDb.Quantity){
                product.Stock -= cartItemDb.Quantity;
                await _productService.SaveChangesAsync();
            }
            else{ return TypedResults.BadRequest("cart-item can not updated"); }

            _cartItemService.Update(resCartItem);
            await _cartItemService.SaveChangesAsync();

            var resultObtained = _cartItemService.GetAll()
                .Where(cartItem => cartItem.Id == cartItemDb.Id).ToList();

            if (resultObtained.Count == 0) return TypedResults.BadRequest("cart-item can not updated");
            var cartItemDto = CartItemMapper.ToDomain(cartItemDb);
            return TypedResults.Created($"/carts/{cartId}/items/{resultObtained[0].Id}", cartItemDto);
        }

        public async Task<IResult> DeleteCartItems(HttpRequest request, int cartId, int productId)
        {
            Logger.Debug("Delete Cart-Items Handler");

            var cartItems = _cartItemService.GetAll()
                .Where(cartItem => cartItem.CartId == cartId)
                .Where(cartItem => cartItem.ProductId == productId)
                .ToList();

            var cartItem = cartItems[0];
            var product = await _productService.GetByIdAsync(cartItem.ProductId);
            product!.Stock += cartItem.Quantity;
            await _productService.SaveChangesAsync();

            await _cartItemService.DeleteByIdAsync(cartItem.Id);
            await _cartItemService.SaveChangesAsync();

            return TypedResults.Empty;
        }
    }
}