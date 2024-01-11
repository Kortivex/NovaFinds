// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductConfig.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ProductConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DAL.Config
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using CORE.Domain;

    /// <summary>
    /// The product config.
    /// </summary>
    internal class ProductConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductConfig"/> class.
        /// </summary>
        /// <param name="eb">
        /// The entity builder.
        /// </param>
        public ProductConfig(EntityTypeBuilder<Product> eb)
        {
            eb.ToTable("Product").HasKey(t => t.Id);

            eb.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();

            eb.Property(t => t.Name).HasMaxLength(256).IsRequired();

            eb.Property(t => t.Description).HasMaxLength(512).IsRequired();

            eb.Property(t => t.Brand).HasMaxLength(512).IsRequired();

            eb.Property(t => t.Price).IsRequired();

            eb.Property(t => t.Stock).IsRequired();

            eb.Property(t => t.CategoryId).IsRequired();

            eb.Property(t => t.ConcurrencyStamp).IsRequired().IsRowVersion();

            // Inner Relation(s).
            eb.HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey("CategoryId")
                .HasPrincipalKey(nameof(Category.Id));
            
            
            // External Relation(s).
            eb.HasMany(product => product.ProductImages)
                .WithOne(productImage => productImage.Product)
                .HasForeignKey("ProductId")
                .HasPrincipalKey(nameof(Product.Id));
            
            eb.HasMany(product => product.CartItems)
                .WithOne(cartItem => cartItem.Product)
                .HasForeignKey("ProductId")
                .HasPrincipalKey(nameof(Product.Id));
            
        }
    }
}