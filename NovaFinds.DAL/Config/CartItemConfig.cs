// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartItemConfig.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CartItemConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DAL.Config
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using CORE.Domain;

    /// <summary>
    /// The cart item config.
    /// </summary>
    internal class CartItemConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemConfig"/> class.
        /// </summary>
        /// <param name="eb">
        /// The entity builder.
        /// </param>
        public CartItemConfig(EntityTypeBuilder<CartItem> eb)
        {
            eb.ToTable("CartItem").HasKey(t => t.Id);

            eb.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();

            eb.Property(t => t.CartId).IsRequired();

            eb.Property(t => t.ProductId).IsRequired();

            eb.Property(t => t.Quantity).IsRequired();

            eb.Property(t => t.ConcurrencyStamp).IsRequired().IsRowVersion();

            // Inner Relation(s).
            eb.HasOne(cartItem => cartItem.Cart)
                .WithMany(cart => cart.CartItems)
                .HasForeignKey("CartId")
                .HasPrincipalKey(nameof(Cart.Id));

            eb.HasOne(cartItem => cartItem.Product)
                .WithMany(product => product.CartItems)
                .HasForeignKey("ProductId")
                .HasPrincipalKey(nameof(Product.Id));
        }
    }
}