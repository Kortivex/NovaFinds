// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Url.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Url type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.MVC.Utils
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// The url utils.
    /// </summary>
    public static class Url
    {
        /// <summary>
        /// The check element in query string.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <param name="replacement">
        /// The replacement.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static string CheckElementInQueryString(string query, string element, string pattern, string replacement)
        {
            if (string.IsNullOrEmpty(query)) throw new ArgumentNullException(nameof(query));

            if (query.Contains($"{element}=", StringComparison.InvariantCulture))
                return Regex.Replace(query, pattern, $"{element}={replacement}");

            var wildcard = query.Contains('?', StringComparison.InvariantCulture) ? "&" : "?";
            return $"{query}{wildcard}{element}={replacement}";
        }

        /// <summary>
        /// The check last slash.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static string CheckLastSlash(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            return path.EndsWith('/') ? path.Remove(path.Length - 1) : path;

        }
    }
}