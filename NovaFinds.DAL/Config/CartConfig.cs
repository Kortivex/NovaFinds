// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartConfig.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CartConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DAL.Config
{
    using CORE.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The cart config.
    /// </summary>
    internal class CartConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartConfig"/> class.
        /// </summary>
        /// <param name="eb">
        /// The entity builder.
        /// </param>
        public CartConfig(EntityTypeBuilder<Cart> eb)
        {
            eb.ToTable("Cart").HasKey(t => t.Id);

            eb.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();

            eb.Property(t => t.UserId).IsRequired();

            eb.Property(t => t.Date).IsRequired();

            eb.Property(t => t.ConcurrencyStamp).IsRequired().IsRowVersion();

            // Inner Relation(s).
            eb.HasOne(cart => cart.User)
                .WithMany(user => user.Carts)
                .HasForeignKey("UserId")
                .HasPrincipalKey(nameof(User.Id));
            
            // External Relation(s).
            eb.HasMany(cart => cart.CartItems)
                .WithOne(cartItem => cartItem.Cart)
                .HasForeignKey("CartId")
                .HasPrincipalKey(nameof(Cart.Id));
        }
    }
}