// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleHandler.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Role Handler type.
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
    public class RoleHandler(IDbContext context, RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        private static int _size = 1000;

        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService _roleService = new(context, roleManager);

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService _userService = new(context, userManager);

        public async Task<IResult> PostRole(HttpRequest request)
        {
            Logger.Debug("Post Role Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqRoleDto = JsonSerializer.Deserialize<RoleDto>(body);
            var roleDb = RoleMapper.ToDb(reqRoleDto!);

            var roleRes = roleManager.CreateAsync(roleDb!).Result;
            if (roleRes.Succeeded){
                var resultObtained = _roleService.GetAll()
                    .Where(role => role.Name == roleDb!.Name).ToList();
                if (resultObtained.Count != 0){
                    reqRoleDto!.Id = resultObtained[0].Id;
                    return TypedResults.Created($"/roles/{resultObtained[0].Id}", reqRoleDto);
                }
                return TypedResults.BadRequest("role can not be created!");
            }
            return TypedResults.BadRequest(roleRes.Errors);
        }

        public async Task<IResult> PutRole(HttpRequest request, int id)
        {
            Logger.Debug("Put Role Handler");
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var reqRoleDto = JsonSerializer.Deserialize<RoleDto>(body);
            var roleDb = RoleMapper.ToDb(reqRoleDto!);

            var role = roleManager.FindByIdAsync(id.ToString()).Result;

            if (role == null) return TypedResults.BadRequest("role can not be updated");

            role.Name = roleDb!.Name;

            await roleManager.UpdateAsync(role);
            await roleManager.UpdateNormalizedRoleNameAsync(role);

            roleDb.Id = role.Id;
            var roleDto = RoleMapper.ToDomain(roleDb);
            return TypedResults.Created($"/roles/{id}", roleDto);
        }

        public IEnumerable<RoleDto?> GetRoles(HttpRequest request)
        {
            Logger.Debug("List Roles Handler");
            var roles = _roleService.GetAll().ToList();

            if (request.Query.ContainsKey("size") && request.Query.ContainsKey("sortBy") && request.Query.ContainsKey("page")){
                _size = int.Parse(request.Query["size"]!);
                var sortBy = request.Query["sortBy"];
                var page = int.Parse(request.Query["page"]!);

                var rolesQuery = _roleService.GetAll().GetPaged(page, _size);
                if (sortBy == "id"){ rolesQuery = rolesQuery.OrderByDescending(o => o.Id); }
                roles = rolesQuery.ToList();
            }

            return RoleMapper.ToListDomain(roles);
        }

        public async Task<IResult> GetRole(HttpRequest request, int id)
        {
            Logger.Debug("Get Role Handler");
            var roles = _roleService.GetAll()
                .Where(role => role.Id == id).ToList();

            if (roles.Count == 0){ return TypedResults.NotFound(); }

            return TypedResults.Ok(RoleMapper.ToDomain(roles[0]));
        }

        public async Task<IResult> GetRoleUsers(HttpRequest request, int id)
        {
            Logger.Debug("Get Role - Users Handler");
            var roles = _roleService.GetAll()
                .Where(role => role.Id == id).ToList();

            if (roles.Count == 0){ return TypedResults.NotFound(); }

            var usersInRole = await userManager.GetUsersInRoleAsync(roles[0].Name!);

            return TypedResults.Ok(UserMapper.ToListDomain(usersInRole));
        }

        public async Task<IResult> DeleteRole(HttpRequest request, int id)
        {
            Logger.Debug("Delete Role Handler");
            var roles = _roleService.GetAll()
                .Where(role => role.Id == id).ToList();

            if (roles.Count == 0){ return TypedResults.NotFound(); }

            var role = roles[0];

            await _roleService.DeleteByIdAsync(role.Id);
            await _roleService.SaveChangesAsync();

            return TypedResults.Empty;
        }

        // USERNAME - ROLE
        public async Task<IResult> GetUserRole(HttpRequest request, string username)
        {
            Logger.Debug("Get User - Role Handler");
            var user = await userManager.FindByNameAsync(username) ?? await userManager.FindByEmailAsync(username);
            if (user == null){ return TypedResults.NotFound("username not found"); }

            var rolesResult = await userManager.GetRolesAsync(user);

            var rolesDb = new List<Role>();
            foreach (var role in rolesResult){
                var roles = _roleService.GetAll()
                    .Where(r => r.Name == role).ToList();
                rolesDb.Add(roles[0]);
            }
            return TypedResults.Ok(RoleMapper.ToListDomain(rolesDb));
        }

        public async Task<IResult> PutUserRole(HttpRequest request, string username, int id)
        {
            Logger.Debug("Put User - Role Handler");
            var user = await userManager.FindByNameAsync(username) ?? await userManager.FindByEmailAsync(username);
            if (user == null){ return TypedResults.NotFound("username not found"); }

            var roles = _roleService.GetAll()
                .Where(role => role.Id == id).ToList();

            if (roles.Count == 0){ return TypedResults.NotFound("role not found"); }
            var role = roles[0];

            var roleAssignResult = await userManager.AddToRoleAsync(user, role.Name!);
            if (roleAssignResult.Succeeded){ return TypedResults.Ok(); }
            return TypedResults.BadRequest("role can not be associated to the user");
        }

        public async Task<IResult> DeleteUserRole(HttpRequest request, string username, int id)
        {
            Logger.Debug("Delete User - Role Handler");
            var user = await userManager.FindByNameAsync(username) ?? await userManager.FindByEmailAsync(username);
            if (user == null){ return TypedResults.NotFound("username not found"); }

            var roles = _roleService.GetAll()
                .Where(role => role.Id == id).ToList();

            if (roles.Count == 0){ return TypedResults.NotFound("role not found"); }
            var role = roles[0];

            var roleAssignResult = await userManager.RemoveFromRoleAsync(user, role.Name!);
            if (roleAssignResult.Succeeded){ return TypedResults.NoContent(); }
            return TypedResults.BadRequest("role can not be removed from the user");
        }
    }
}