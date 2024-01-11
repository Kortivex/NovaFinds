// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICartItemRepository.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ICartItemRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Contracts
{
    using Domain;

    /// <summary>
    /// The ICartItemRepository interface.
    /// </summary>
    public interface ICartItemRepository : IRepository<CartItem>
    {
    }
}