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
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// The user service.
    /// </summary>
    public class UserService : Service<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="userManager">
        /// The UserManager from Identity.
        /// </param>
        public UserService(IDbContext context, UserManager<User> userManager) : base(context)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Get the specified <paramref name="username"/> in the backing store.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <returns>
        /// The <see cref="IdentityResult"/>.
        /// </returns>
        public async Task<User?> GetByUserNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        /// <summary>
        /// Creates the specified <paramref name="user"/> in the backing store with given password.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="password">
        /// The user password.
        /// </param>
        /// <returns>
        /// The <see cref="IdentityResult"/>.
        /// </returns>
        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}