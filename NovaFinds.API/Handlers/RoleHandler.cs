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
    public class RoleHandler(IDbContext context, RoleManager<Role> roleManager)
    {
        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService _roleService = new(context, roleManager);


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
    }
}