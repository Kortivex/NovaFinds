// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Category.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Category type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Domain
{
    using Contracts;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Category table
    /// </summary>
    /// <remarks>
    ///     Table with category of the Product
    /// </remarks>
    public sealed class Category : IEntity
    {
        /// <summary>
        ///     Public constructor with required data
        /// </summary>
        /// <param name="name">Name of the category</param>
        public Category(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            this.Name = name;
            this.ConcurrencyStamp = new byte[32];

            this.Products = new List<Product>();
        }

        /// <summary>
        ///     Default constructor. Present because EF needs it.
        /// </summary>
        public Category()
        {
            this.Name = "";
            this.ConcurrencyStamp = new byte[32];

            this.Products = new List<Product>();
        }

        /// <summary>
        ///     Identity, Indexed, Required
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Category
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        ///     Required, Max length = 256
        ///     Name of the category
        /// </summary>
        /// <remarks>
        ///     Detailed association name of the category
        /// </remarks>
        [StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        ///     Required
        ///     Concurrency Token
        /// </summary>
        /// <remarks>
        ///     Concurrency Control
        /// </remarks>
        public byte[] ConcurrencyStamp { get; set; }

        /// <summary>
        ///     Required
        /// </summary>
        /// <remarks>
        ///     Relation Category & Product
        /// </remarks>
        public IEnumerable<Product> Products { get; }

        /// <summary>
        ///     Static create function (for use in LINQ queries, etc.)
        /// </summary>
        /// <param name="name">Name of the category</param>
        public static Category Create(string name)
        {
            return new Category(name);
        }
    }
}