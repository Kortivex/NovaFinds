// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICartRepository.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ICartRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Contracts
{
    using Domain;

    /// <summary>
    /// The CartRepository interface.
    /// </summary>
    public interface ICartRepository : IRepository<Cart>
    {
    }
}