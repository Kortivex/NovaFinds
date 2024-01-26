// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="">
//
// </copyright>
// <summary>
//   Defines the IUserRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Contracts
{
    using Domain;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// The UserRepository interface.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {

        /// <summary>
        /// Get the specified <paramref name="username"/> in the backing store.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <returns>
        /// The <see cref="IdentityResult"/>.
        /// </returns>
        public Task<User?> GetByUserNameAsync(string username);
        
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
        public Task<IdentityResult> CreateUserAsync(User user, string password);
    }
}