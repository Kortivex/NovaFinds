// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageNavPages.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ManageNavPages type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Identity.Pages.Account.Manage
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// The manage nav pages.
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        /// The address.
        /// </summary>
        public static string Address => "Address";

        /// <summary>
        /// The change password.
        /// </summary>
        public static string ChangePassword => "ChangePassword";

        /// <summary>
        /// The index.
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        /// The orders.
        /// </summary>
        public static string Orders => "Orders";

        /// <summary>
        /// The change address nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string? ChangeAddressNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, Address);
        }

        /// <summary>
        /// The change password nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string? ChangePasswordNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, ChangePassword);
        }

        /// <summary>
        /// The index nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string? IndexNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, Index);
        }

        /// <summary>
        /// The orders nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string? OrdersNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, Orders);
        }

        /// <summary>
        /// The page nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string? PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                             ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}