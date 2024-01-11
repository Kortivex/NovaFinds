// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDbContext.cs" company="">
//
// </copyright>
// <summary>
//   Defines the IDbContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Contracts
{
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public interface IDbContext
    {
        /// <summary>
        ///     Repository for Cart - Shopping Cart table
        /// </summary>
        DbSet<Cart> Carts { get; set; }

        /// <summary>
        ///     Repository for CartItem - Shopping Cart-Item table
        /// </summary>
        DbSet<CartItem> CartItems { get; set; }

        /// <summary>
        ///     Repository for Order - Order table
        /// </summary>
        DbSet<Order> Orders { get; set; }

        /// <summary>
        ///     Repository for Product - Product table
        /// </summary>
        DbSet<Product> Products { get; set; }

        /// <summary>
        ///     Repository for Category - Category table
        /// </summary>
        DbSet<Category> Categories { get; set; }

        /// <summary>
        ///     Repository for ProductImage - Product-Image table
        /// </summary>
        DbSet<ProductImage> ProductImages { get; set; }

        /// <summary>
        ///     Repository for OrderProduct - Order-Product table
        /// </summary>
        DbSet<OrderProduct> OrderProducts { get; set; }

        /// <summary>
        ///     Repository for User table
        /// </summary>
        DbSet<User> Users { get; set; }

        /// <summary>
        ///     Repository for Role table
        /// </summary>
        DbSet<Role> Roles { get; set; }
        
        /// <summary>
        ///     Db rep. for ApiKeys
        /// </summary>
        DbSet<ApiKey> ApiKeys { get; set; }

        /// <summary>
        /// Get collection from an Entity
        /// </summary>
        /// <typeparam name="T">Entity related to the Context</typeparam>
        /// <returns>Entity collection</returns>
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Performs tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
        /// </summary>
        ValueTask DisposeAsync();

        /// <summary>
        /// Save changes in async way.
        /// </summary>
        /// <returns>Elements affected</returns>
        Task<int> SaveChangesAsync();
    }
}