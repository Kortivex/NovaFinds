// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserConfig.cs" company="">
//
// </copyright>
// <summary>
//   Defines the UserConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DAL.Config
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using CORE.Domain;

    /// <summary>
    /// The user config.
    /// </summary>
    internal class UserConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserConfig"/> class.
        /// </summary>
        /// <param name="eb">
        /// The entity builder.
        /// </param>
        public UserConfig(EntityTypeBuilder<User> eb)
        {
            eb.ToTable("User");

            eb.Property(t => t.Nif).HasMaxLength(9).IsRequired();

            eb.Property(t => t.FirstName).HasMaxLength(256).IsRequired();

            eb.Property(t => t.LastName).HasMaxLength(256).IsRequired();

            eb.Property(t => t.StreetAddress).HasMaxLength(256).IsRequired();

            eb.Property(t => t.City).HasMaxLength(128).IsRequired();

            eb.Property(t => t.State).HasMaxLength(128).IsRequired();

            eb.Property(t => t.Country).HasMaxLength(128).IsRequired();

            eb.Property(t => t.Email).HasMaxLength(256).IsRequired();

            // External Relation(s).
            eb.HasMany(user => user.Carts)
                .WithOne(cart => cart.User)
                .HasForeignKey("UserId")
                .HasPrincipalKey(nameof(User.Id));

            eb.HasMany(user => user.Orders)
                .WithOne(order => order.User)
                .HasForeignKey("UserId")
                .HasPrincipalKey(nameof(User.Id));
        }
    }
}