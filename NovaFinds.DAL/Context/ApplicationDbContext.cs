// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationDbContext.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ApplicationDbContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DAL.Context
{
    using Config;
    using CORE.Contracts;
    using CORE.Domain;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The application db context.
    /// </summary>
    public sealed class ApplicationDbContext : IdentityDbContext<User, Role, int>, IDbContext
    {
        public ApplicationDbContext() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">
        /// The db context options.
        /// </param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        /// <summary>
        ///     Repository for Cart - Shopping Cart table
        /// </summary>
        public DbSet<Cart>? Carts { get; set; }

        /// <summary>
        ///     Repository for CartItem - Shopping Cart-Item table
        /// </summary>
        public DbSet<CartItem>? CartItems { get; set; }

        /// <summary>
        ///     Repository for Order - Order table
        /// </summary>
        public DbSet<Order>? Orders { get; set; }

        /// <summary>
        ///     Repository for Product - Product table
        /// </summary>
        public DbSet<Product>? Products { get; set; }

        /// <summary>
        ///     Repository for Category - Category table
        /// </summary>
        public DbSet<Category>? Categories { get; set; }

        /// <summary>
        ///     Repository for ProductImage - Product-Image table
        /// </summary>
        public DbSet<ProductImage>? ProductImages { get; set; }

        /// <summary>
        ///     Repository for OrderProduct - Order-Product table
        /// </summary>
        public DbSet<OrderProduct>? OrderProducts { get; set; }

        /// <summary>
        ///     Repository for User table
        /// </summary>
        public new DbSet<User>? Users { get; set; }

        /// <summary>
        ///     Repository for Role table
        /// </summary>
        public new DbSet<Role>? Roles { get; set; }
        
        /// <summary>
        ///     Db rep. for ApiKeys
        /// </summary>
        public DbSet<ApiKey>? ApiKeys { get; set; }
        
        /// <summary>
        ///  Saves all changes made in this context to the database.
        /// </summary>
        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            _ = new ApiKeyConfig(builder.Entity<ApiKey>());
            _ = new CartConfig(builder.Entity<Cart>());
            _ = new CartItemConfig(builder.Entity<CartItem>());
            _ = new OrderConfig(builder.Entity<Order>());
            _ = new OrderProductConfig(builder.Entity<OrderProduct>());
            _ = new ProductConfig(builder.Entity<Product>());
            _ = new CategoryConfig(builder.Entity<Category>());
            _ = new ProductImageConfig(builder.Entity<ProductImage>());
            _ = new UserConfig(builder.Entity<User>());

            base.OnModelCreating(builder);
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}