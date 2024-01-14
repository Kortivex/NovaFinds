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
        public Role()
        {
            this.Name = "";
            this.ConcurrencyStamp = new byte[32];
        }

        [StringLength(512)]
        public new string Name { get; set; }

        /// <summary>
        ///     Required
        ///     Concurrency Token
        /// </summary>
        /// <remarks>
        ///     Concurrency Control
        /// </remarks>
        public new byte[] ConcurrencyStamp { get; set; }
    }
}