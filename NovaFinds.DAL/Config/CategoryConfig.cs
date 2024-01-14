// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryConfig.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CategoryConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DAL.Config
{
    using CORE.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The category config.
    /// </summary>
    internal class CategoryConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryConfig"/> class.
        /// </summary>
        /// <param name="eb">
        /// The entity builder.
        /// </param>
        public CategoryConfig(EntityTypeBuilder<Category> eb)
        {
            eb.ToTable("Category").HasKey(t => t.Id);

            eb.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();

            eb.Property(t => t.Name).HasMaxLength(256).IsRequired();

            eb.Property(t => t.ConcurrencyStamp).IsRequired().IsRowVersion();

            // External Relation(s).
            eb.HasMany(category => category.Products)
                .WithOne(product => product.Category)
                .HasForeignKey("CategoryId")
                .HasPrincipalKey(nameof(Category.Id));
        }
    }
}