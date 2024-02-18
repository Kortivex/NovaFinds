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
    using Application.Mappers;
    using Application.Services;
    using Auth;
    using CORE.Contracts;
    using CORE.Domain;
    using DTOs;
    using Filters;
    using IFR.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using System.Text.Json;

    [Authorize(AuthenticationSchemes = ApiKeySchemeOptions.AuthenticateScheme)]
    public class UserHandler(IDbContext context, UserManager<User> userManager)
    {
        private static int _size = 1000;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService _userService = new(context, userManager);

        /// <summary>
        /// The cart service.
        /// </summary>
        private readonly CartService _cartService = new(context);

        /// <summary>
        /// The order service.
        /// </summary>
        private readonly OrderService _orderService = new(context);

        public async Task<IResult> PostUser(HttpRequest request)
        {
            Logger.Debug("Post User Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqUserDto = JsonSerializer.Deserialize<UserDto>(body);
            var userDb = UserMapper.ToDb(reqUserDto!);

            var resultCreated = await _userService.CreateUserAsync(userDb!, userDb!.Password!);
            var resultObtained = await _userService.GetByUserNameAsync(userDb.UserName!);

            if (resultCreated.Succeeded){ return TypedResults.Created($"/users/{resultObtained!.Id}", reqUserDto); }
            return TypedResults.BadRequest(resultCreated.Errors);
        }

        public async Task<IResult> PutUser(HttpRequest request, string username)
        {
            Logger.Debug("Put User Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqUserDto = JsonSerializer.Deserialize<UserDto>(body);
            var userDb = UserMapper.ToDb(reqUserDto!);

            var user = userManager.FindByNameAsync(username).Result ?? userManager.FindByEmailAsync(username).Result;

            if (user == null) return TypedResults.BadRequest("user can not be updated");

            user.UserName = userDb!.UserName;
            user.Password = userDb.Password;
            user.FirstName = userDb.FirstName;
            user.LastName = userDb.LastName;
            user.Email = userDb.Email;
            user.EmailConfirmed = userDb.EmailConfirmed;
            user.Nif = userDb.Nif;
            user.City = userDb.City;
            user.Country = userDb.Country;
            user.State = userDb.State;
            user.StreetAddress = userDb.StreetAddress;
            user.PhoneNumber = userDb.PhoneNumber;
            user.PhoneNumberConfirmed = userDb.PhoneNumberConfirmed;
            user.IsActive = userDb.IsActive;

            if (!string.IsNullOrEmpty(userDb.Password)){
                await userManager.RemovePasswordAsync(user);
                await userManager.AddPasswordAsync(user, user.Password!);
            }

            await userManager.UpdateAsync(user);

            var userDto = UserMapper.ToDomain(userDb);
            return TypedResults.Created($"/users/{username}", userDto);
        }

        public IEnumerable<UserDto?> GetUsers(HttpRequest request)
        {
            Logger.Debug("List Users Handler");
            var users = _userService.GetAll().ToList();

            if (request.Query.ContainsKey("size") && request.Query.ContainsKey("sortBy") && request.Query.ContainsKey("page")){
                _size = int.Parse(request.Query["size"]!);
                var sortBy = request.Query["sortBy"];
                var page = int.Parse(request.Query["page"]!);

                var usersQuery = _userService.GetAll().GetPaged(page, _size);
                if (sortBy == "id"){ usersQuery = usersQuery.OrderByDescending(o => o.Id); }
                users = usersQuery.ToList();
            }
            else{
                if (request.Query.ContainsKey("username")){
                    var username = request.Query["username"].ToString();
                    users = _userService.GetAll()
                        .Where(user => user.UserName == username).ToList();
                }
                if (!request.Query.ContainsKey("email")) return UserMapper.ToListDomain(users);
                var email = request.Query["email"].ToString();
                users = _userService.GetAll()
                    .Where(user => user.Email == email).ToList();
            }

            return UserMapper.ToListDomain(users);
        }

        public async Task<IResult> GetUser(HttpRequest request, int id)
        {
            Logger.Debug("Get User by Id Handler");
            var user = await _userService.GetByIdAsync(id);
            if (user == null){ return TypedResults.NotFound(); }

            return TypedResults.Ok(UserMapper.ToDomain(user));
        }

        public async Task<IResult> GetUser(HttpRequest request, string username)
        {
            Logger.Debug("Get User by Username Handler");
            var users = _userService.GetAll()
                .Where(user => user.UserName == username).ToList();
            if (users.Count == 0){
                users = _userService.GetAll()
                    .Where(user => user.Email == username).ToList();
            }

            if (users.Count == 0){ return TypedResults.NotFound(); }

            return TypedResults.Ok(UserMapper.ToDomain(users[0]));
        }

        public async Task<IResult> DeleteUser(HttpRequest request, string username)
        {
            Logger.Debug("Delete User Handler");
            var users = _userService.GetAll()
                .Where(user => user.UserName == username).ToList();
            var user = users[0];

            await _userService.DeleteByIdAsync(user.Id);
            await _userService.SaveChangesAsync();

            return TypedResults.Empty;
        }

        // CARTS
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

        // ORDERS
        public IEnumerable<OrderDto?> GetUsersOrders(HttpRequest request, string username)
        {
            Logger.Debug("List User-Orders Handler");
            List<Order> orders;
            var users = _userService.GetAll()
                .Where(user => user.UserName == username).ToList();

            if (users.Count != 0){
                orders = _orderService.GetAll()
                    .Where(order => order.UserId == users[0].Id).ToList();
            }
            else{
                users = _userService.GetAll()
                    .Where(user => user.Email == username).ToList();
                orders = _orderService.GetAll()
                    .Where(order => order.UserId == users[0].Id).ToList();
            }

            return OrderMapper.ToListDomain(orders);
        }

        public async Task<IResult> PostUsersOrders(HttpRequest request, string username)
        {
            Logger.Debug("Post Users-Orders Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqOrderDto = JsonSerializer.Deserialize<OrderDto>(body);
            var orderDb = OrderMapper.ToDb(reqOrderDto!);

            var users = _userService.GetAll()
                .Where(user => user.UserName == username).ToList();

            if (users.Count == 0) return TypedResults.BadRequest("username not exist");
            {
                var user = users[0];
                orderDb!.UserId = user.Id;
                await _orderService.CreateAsync(orderDb);
                await _orderService.SaveChangesAsync();

                var resultObtained = _orderService.GetAll()
                    .Where(order => order.UserId == user.Id).ToList();

                if (resultObtained.Count == 0) return TypedResults.BadRequest("order can not created");
                var orderDto = OrderMapper.ToDomain(orderDb);
                orderDto!.UserId = user.Id;
                return TypedResults.Created($"/users/{user.UserName}/orders/{resultObtained[0].Id}", orderDto);
            }
        }

        public async Task<IResult> PutUsersOrders(HttpRequest request, string username, int id)
        {
            Logger.Debug("Put Users-Orders Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqOrderDto = JsonSerializer.Deserialize<OrderDto>(body);
            var orderDb = OrderMapper.ToDb(reqOrderDto!);

            var users = _userService.GetAll()
                .Where(user => user.UserName == username).ToList();
            await _userService.SaveChangesAsync();

            if (users.Count == 0) return TypedResults.BadRequest("username not exist");
            {
                var user = users[0];
                var orderExist = _orderService.GetByIdAsync(id).Result;
                if (orderExist != null && orderExist.UserId == user.Id){
                    orderExist.Status = orderDb!.Status;
                    await _orderService.SaveChangesAsync();

                    var resultObtained = _orderService.GetAll()
                        .Where(order => order.UserId == user.Id)
                        .Where(order => order.Id == id)
                        .ToList();

                    if (resultObtained.Count == 0) return TypedResults.BadRequest("order can not updated");
                    var orderDto = OrderMapper.ToDomain(orderDb);
                    orderDto!.Id = id;
                    orderDto.UserId = user.Id;
                    return TypedResults.Created($"/users/{user.UserName}/orders/{resultObtained[0].Id}", orderDto);
                }
                return TypedResults.BadRequest("order can not updated");
            }
        }
    }
}