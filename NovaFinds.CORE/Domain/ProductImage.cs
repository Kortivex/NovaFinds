// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductImage.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ProductImage type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Domain
{
    using Contracts;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     Product-Image table
    /// </summary>
    /// <remarks>
    ///     Table with image elements associated of the Product
    /// </remarks>
    public sealed class ProductImage : IEntity
    {
        /// <summary>
        ///     Public constructor with required data
        /// </summary>
        /// <param name="image">Representation in b64 of the image</param>
        /// <param name="description">Description of the image</param>
        /// <param name="productId">Product Id</param>
        public ProductImage(
            string image,
            string description,
            int productId)
        {
            if (string.IsNullOrEmpty(image)) throw new ArgumentNullException(nameof(image));
            this.Image = image;
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));
            this.Description = description;
            this.ProductId = productId;
            this.ConcurrencyStamp = new byte[32];

            this.Product = new Product();
        }

        /// <summary>
        ///     Default constructor. Present because EF needs it.
        /// </summary>
        public ProductImage()
        {
            this.Image = "";
            this.Description = "-";
            this.ConcurrencyStamp = new byte[32];

            this.Product = new Product();
        }

        /// <summary>
        ///     Identity, Indexed, Required
        ///     Entity Identifier
        /// </summary>
        /// <remarks>
        ///     Entity Identifier of Product-Media
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        ///     Required
        ///     Image to load
        /// </summary>
        /// <remarks>
        ///     Image in base64 to load
        /// </remarks>
        public string Image { get; set; }

        /// <summary>
        ///     Max length = 256, Default value = "-"
        ///     Description of the image
        /// </summary>
        /// <remarks>
        ///     Detailed Description of the image
        /// </remarks>
        [StringLength(256)]
        public string Description { get; set; }

        /// <summary>
        ///     Required
        ///     External Product Ref.
        /// </summary>
        /// <remarks>
        ///     FK of the related Product
        /// </remarks>
        public int ProductId { get; set; }

        /// <summary>
        ///     Required
        ///     Concurrency Token
        /// </summary>
        /// <remarks>
        ///     Concurrency Control
        /// </remarks>
        public byte[] ConcurrencyStamp { get; set; }

        /// <summary>
        ///     Required
        /// </summary>
        /// <remarks>
        ///     Relation Product-Image & Product
        /// </remarks>
        public Product Product { get; set; }

        /// <summary>
        ///     Static create function (for use in LINQ queries, etc.)
        /// </summary>
        /// <param name="image">Representation in b64 of the image</param>
        /// <param name="description">Description of the image</param>
        /// <param name="productId">Product Id</param>
        public static ProductImage Create(
            string image,
            string description,
            int productId)
        {
            return new ProductImage(image, description, productId);
        }
    }
}