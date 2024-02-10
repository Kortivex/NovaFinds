// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Role.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Role type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Domain
{
    using Contracts;
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The role.
    /// </summary>
    public class Role : IdentityRole<int>, IEntity
    {
        /// <summary>
        ///     Default constructor. Required because EF needs it.
        /// </summary>
        public Role() {}
    }
}