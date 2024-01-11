// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderStatusType.cs" company="">
//
// </copyright>
// <summary>
//   Entity Type of the Order-Status
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.CORE.Enums
{
    /// <summary>
    ///     Entity Type of the Order-Status
    /// </summary>
    /// <remarks>
    ///     Representation of the Order-Status
    /// </remarks>
    public enum OrderStatusType
    {
        /// <summary>
        /// The pending.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The paid.
        /// </summary>
        Paid = 1,

        /// <summary>
        /// The processing.
        /// </summary>
        Processing = 2,

        /// <summary>
        /// The shipped.
        /// </summary>
        Shipped = 3,

        /// <summary>
        /// The delivered.
        /// </summary>
        Delivered = 4
    }
}