// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryService.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CategoryService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.Application.Services
{
    using CORE.Contracts;
    using CORE.Domain;

    /// <summary>
    /// The category service.
    /// </summary>
    public class CategoryService: Service<Category>, ICategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public CategoryService(IDbContext context) : base(context)
        {
        }
    }
}