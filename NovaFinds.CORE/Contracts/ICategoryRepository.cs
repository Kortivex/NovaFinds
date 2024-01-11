// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategoryRepository.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ICategoryRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Contracts
{
    using Domain;

    /// <summary>
    /// The ICategoryRepository interface.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
    }
}