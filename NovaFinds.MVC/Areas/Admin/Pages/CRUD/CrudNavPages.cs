// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrudNavPages.cs" company="">
//
// </copyright>
// <summary>
//   Defines the CrudNavPages type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Areas.Admin.Pages.CRUD
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// The crud nav pages.
    /// </summary>
    public static class CrudNavPages
    {
        /// <summary>
        /// The index brands nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string IndexBrandsNavClass(ViewContext viewContext)
        {
            return PageCrudNavClass(viewContext, "Index");
        }

        /// <summary>
        /// The index categories nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string IndexCategoriesNavClass(ViewContext viewContext)
        {
            return PageCrudNavClass(viewContext, "Index");
        }

        /// <summary>
        /// The index media nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string IndexMediaNavClass(ViewContext viewContext)
        {
            return PageCrudNavClass(viewContext, "Index");
        }

        /// <summary>
        /// The index orders nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string IndexOrdersNavClass(ViewContext viewContext)
        {
            return PageCrudNavClass(viewContext, "Index");
        }

        /// <summary>
        /// The index product nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string IndexProductNavClass(ViewContext viewContext)
        {
            return PageCrudNavClass(viewContext, "Index");
        }

        /// <summary>
        /// The index tax nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string IndexTaxNavClass(ViewContext viewContext)
        {
            return PageCrudNavClass(viewContext, "Index");
        }

        /// <summary>
        /// The index users nav class.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string IndexUsersNavClass(ViewContext viewContext)
        {
            return PageCrudNavClass(viewContext, "Index");
        }

        /// <summary>
        /// The page crud nav class.
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
        private static string PageCrudNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                             ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return (string.Equals(activePage!, page, StringComparison.OrdinalIgnoreCase) ? "active" : null)!;
        }
    }
}