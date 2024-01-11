// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserService.cs" company="">
//
// </copyright>
// <summary>
//   The user service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Services
{
    using CORE.Contracts;
    using CORE.Domain;
    
    /// <summary>
    /// The user service.
    /// </summary>
    public class UserService : Service<User>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public UserService(IDbContext context) : base(context) {}
    }
}