// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderConfig.cs" company="">
//
// </copyright>
// <summary>
//   Defines the OrderConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DAL.Config
{
    using CORE.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The order config.
    /// </summary>
    internal class OrderConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderConfig"/> class.
        /// </summary>
        /// <param name="eb">
        /// The entity builder.
        /// </param>
        public OrderConfig(EntityTypeBuilder<Order> eb)
        {
            eb.ToTable("Order").HasKey(t => t.Id);

            eb.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();

            eb.Property(t => t.UserId).IsRequired();

            eb.Property(t => t.Status).IsRequired();

            eb.Property(t => t.Date).IsRequired();

            eb.Property(t => t.ConcurrencyStamp).IsRequired().IsRowVersion();

            // Inner Relation(s).
            eb.HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey("UserId")
                .HasPrincipalKey(nameof(User.Id));

            // External Relation(s).
            eb.HasMany(order => order.OrderProducts)
                .WithOne(orderProduct => orderProduct.Order)
                .HasForeignKey("OrderId")
                .HasPrincipalKey(nameof(Order.Id));
        }
    }
}