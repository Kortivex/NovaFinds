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
    }
}