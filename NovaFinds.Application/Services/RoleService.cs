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
    
    /// <summary>
    /// The role service.
    /// </summary>
    public class RoleService : Service<Role>, IRoleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public RoleService(IDbContext context) : base(context) {}
    }
}