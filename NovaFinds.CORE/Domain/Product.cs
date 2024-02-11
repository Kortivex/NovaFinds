// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Product type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Domain
{
    using Contracts;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Product table
    /// </summary>
    /// <remarks>
    ///     Table of the Product
    /// </remarks>
    public sealed class Product : IEntity
    {
        /// <summary>
        ///     Public constructor with required data
        /// </summary>
        /// <param name="name">Name of the product</param>
        /// <param name="categoryId">External Product-Category</param>
        /// <param name="description">Full Description of the product</param>
        /// <param name="brand">Brand of the product</param>
        /// <param name="price">Price of the product</param>
        /// <param name="stock">Units available in warehouse</param>
        public Product(
            string name,
            int categoryId,
            string description = "-",
            string brand = "-",
            double price = 0,
            int stock = 0)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            this.Name = name;
            this.Description = description;
            this.Brand = brand;
            this.Price = price;
            this.Stock = stock;
            this.CategoryId = categoryId;
            this.ConcurrencyStamp = new byte[32];

            this.Category = new Category();
            this.CartItems = new List<CartItem>();
            this.ProductImages = new List<ProductImage>();
            this.OrderProducts = new List<OrderProduct>();
        }

        /// <summary>
        ///     Default constructor. Present because EF needs it.
        /// </summary>
        public Product()
        {
            this.Name = "";
            this.Description = "-";
            this.Brand = "-";
            this.Price = 0;
            this.Stock = 0;
            this.CategoryId = 0;
            this.ConcurrencyStamp = new byte[32];

            this.Category = new Category();
            this.CartItems = new List<CartItem>();
            this.ProductImages = new List<ProductImage>();
            this.OrderProducts = new List<OrderProduct>();
        }

        /// <summary>
        ///     Identity, Indexed, Required
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Product
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        ///     Required
        ///     Name of the product
        /// </summary>
        /// <remarks>
        ///     Detailed association name of the product
        /// </remarks>
        [StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        ///     Required, Max length = 512, Default value = "-"
        ///     Full Description of the product
        /// </summary>
        /// <remarks>
        ///     Detailed Description of the product
        /// </remarks>
        [StringLength(512)]
        public string Description { get; set; }

        /// <summary>
        ///     Required
        ///     Brand of the product
        /// </summary>
        /// <remarks>
        ///     Detailed association brand of the product
        /// </remarks>
        [StringLength(512)]
        public string Brand { get; set; }

        /// <summary>
        ///     Required, Default value = 0
        ///     Price of the product
        /// </summary>
        /// <remarks>
        ///     Value of the product
        /// </remarks>
        public double Price { get; set; }

        /// <summary>
        ///     Required, Default value = 0
        ///     Units Available in warehouse
        /// </summary>
        /// <remarks>
        ///     Units in the warehouse
        /// </remarks>
        public int Stock { get; set; }

        /// <summary>
        ///     Required
        ///     External Product-Category Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related category
        /// </remarks>
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        /// <summary>
        ///     Required
        ///     Concurrency Token
        /// </summary>
        /// <remarks>
        ///     Concurrency Control
        /// </remarks>
        public byte[] ConcurrencyStamp { get; set; }

        public Category? Category { get; set; }

        public IEnumerable<CartItem> CartItems { get; }

        public IEnumerable<ProductImage>? ProductImages { get; set; }

        public IEnumerable<OrderProduct> OrderProducts { get; }

        /// <summary>
        ///     Static create function (for use in LINQ queries, etc.)
        /// </summary>
        /// <param name="name">Name of the product</param>
        /// <param name="categoryId">Product-Category association</param>
        /// <param name="description">Description of the product</param>
        /// <param name="brand">Brand of the product</param>
        /// <param name="price">Price of the product</param>
        /// <param name="stock">Units available in warehouse</param>
        /// >
        public static Product Create(
            string name,
            int categoryId,
            string description = "-",
            string brand = "-",
            double price = 0,
            int stock = 0
            )
        {
            return new Product(
                name,
                categoryId,
                description,
                brand,
                price,
                stock
                );
        }
    }
}