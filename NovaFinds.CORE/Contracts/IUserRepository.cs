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

    /// <summary>
    /// The UserRepository interface.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
    }
}