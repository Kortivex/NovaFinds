// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiKeyConfig.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ApiKeyConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.DAL.Config
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using CORE.Domain;

    /// <summary>
    /// The apikey config.
    /// </summary>
    internal class ApiKeyConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyConfig"/> class.
        /// </summary>
        /// <param name="eb">
        /// The entity builder.
        /// </param>
        public ApiKeyConfig(EntityTypeBuilder<ApiKey> eb)
        {
            eb.ToTable("ApiKey").HasKey(t => t.ApiKeyId);

            eb.Property(t => t.ApiKeyId).IsRequired().ValueGeneratedOnAdd();

            eb.Property(t => t.Key).IsRequired();

            eb.Property(t => t.Name).IsRequired();
        }
    }
}