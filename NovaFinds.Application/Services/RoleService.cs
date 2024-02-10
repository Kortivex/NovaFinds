// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleService.cs" company="">
//
// </copyright>
// <summary>
//   The role service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Services
{
    using CORE.Contracts;
    using CORE.Domain;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// The role service.
    /// </summary>
    public class RoleService : Service<Role>, IRoleRepository
    {
        private readonly RoleManager<Role> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="roleManager">
        /// The RoleManager from Identity.
        /// </param>
        public RoleService(IDbContext context, RoleManager<Role> roleManager) : base(context)
        {
            _roleManager = roleManager;
        }
    }
}