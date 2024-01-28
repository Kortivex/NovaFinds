// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserHandler.cs" company="">
//
// </copyright>
// <summary>
//   Defines the User Handler type.
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
    public class UserHandler(IDbContext context, UserManager<User> userManager)
    {
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService _userService = new(context, userManager);

        /// <summary>
        /// The cart service.
        /// </summary>
        private readonly CartService _cartService = new(context);

        public async Task<IResult> PostUser(HttpRequest request)
        {
            Logger.Debug("Post User Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqUserDto = JsonSerializer.Deserialize<UserDto>(body);
            var userDb = UserMapper.ToDb(reqUserDto!);

            var resultCreated = await _userService.CreateUserAsync(userDb!, userDb!.Password);
            var resultObtained = await _userService.GetByUserNameAsync(userDb.UserName!);

            if (resultCreated.Succeeded){ return TypedResults.Created($"/users/{resultObtained!.Id}", reqUserDto); }
            return TypedResults.BadRequest(resultCreated.Errors);
        }

        public IEnumerable<UserDto?> GetUsers(HttpRequest request)
        {
            Logger.Debug("List Users Handler");
            var users = _userService.GetAll().ToList();
            if (request.Query.ContainsKey("username")){
                var username = request.Query["username"].ToString();
                users = _userService.GetAll()
                    .Where(user => user.UserName == username).ToList();
            }
            if (!request.Query.ContainsKey("email")) return UserMapper.ToListDomain(users);
            {
                var email = request.Query["email"].ToString();
                users = _userService.GetAll()
                    .Where(user => user.Email == email).ToList();
            }

            return UserMapper.ToListDomain(users);
        }

        public IEnumerable<CartDto?> GetUsersCart(HttpRequest request, string username)
        {
            Logger.Debug("List User-Carts Handler");
            List<Cart> carts;
            var users = _userService.GetAll()
                .Where(user => user.UserName == username).ToList();

            if (users.Count != 0){
                carts = _cartService.GetAll()
                    .Where(cart => cart.UserId == users[0].Id).ToList();
            }
            else{
                users = _userService.GetAll()
                    .Where(user => user.Email == username).ToList();
                carts = _cartService.GetAll()
                    .Where(cart => cart.UserId == users[0].Id).ToList();
            }

            return CartMapper.ToListDomain(carts);
        }
    }
}