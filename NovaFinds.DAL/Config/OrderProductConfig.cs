// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderProductConfig.cs" company="">
//
// </copyright>
// <summary>
//   Defines the OrderProductConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DAL.Config
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using CORE.Domain;

    /// <summary>
    /// The order product config.
    /// </summary>
    internal class OrderProductConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderProductConfig"/> class.
        /// </summary>
        /// <param name="eb">
        /// The entity builder.
        /// </param>
        public OrderProductConfig(EntityTypeBuilder<OrderProduct> eb)
        {
            eb.ToTable("OrderProduct").HasKey(t => t.Id);

            eb.Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();

            eb.Property(t => t.ProductId).IsRequired();

            eb.Property(t => t.OrderId).IsRequired();

            eb.Property(t => t.Quantity).IsRequired();

            eb.Property(t => t.Total).IsRequired().HasColumnType("decimal(18,3)");

            eb.Property(t => t.ConcurrencyStamp).IsRequired().IsRowVersion();
            
            // Inner Relation(s).
            eb.HasOne(orderProduct => orderProduct.Order)
                .WithMany(order => order.OrderProducts)
                .HasForeignKey("OrderId")
                .HasPrincipalKey(nameof(Order.Id));

            eb.HasOne(orderProduct => orderProduct.Product)
                .WithMany(product => product.OrderProducts)
                .HasForeignKey("ProductId")
                .HasPrincipalKey(nameof(Product.Id));
        }
    }
}