// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductImageConfig.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ProductImageConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DAL.Config
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using CORE.Domain;

    /// <summary>
    /// The product image config.
    /// </summary>
    internal class ProductImageConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductImageConfig"/> class.
        /// </summary>
        /// <param name="eb">
        /// The entity builder.
        /// </param>
        public ProductImageConfig(EntityTypeBuilder<ProductImage> eb)
        {
            eb.ToTable("ProductImage").HasKey(t => t.Id);

            eb.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();

            eb.Property(t => t.Image).IsRequired();

            eb.Property(t => t.Description).HasMaxLength(256);

            eb.Property(t => t.ProductId).IsRequired();

            eb.Property(t => t.ConcurrencyStamp).IsRequired().IsRowVersion();

            // Inner Relation(s).
            eb.HasOne(productImage => productImage.Product)
                .WithMany(product => product.ProductImages)
                .HasForeignKey("ProductId")
                .HasPrincipalKey(nameof(Product.Id));
        }
    }
}