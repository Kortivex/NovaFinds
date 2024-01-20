// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Paginator.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Paginator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NovaFinds.API.Filters
{

    /// <summary>
    /// The paginator for different zones of page.
    /// </summary>
    public class Paginator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Paginator"/> class.
        /// </summary>
        /// <param name="totalItems">
        /// The total items.
        /// </param>
        /// <param name="currentPage">
        /// The current page.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="maxPages">
        /// The max pages.
        /// </param>
        public Paginator(int totalItems, int currentPage = 1, int pageSize = 10, int maxPages = 10)
        {
            this.Pages = new List<int>();
            CalculatePages(totalItems, currentPage, pageSize, maxPages);
        }

        /// <summary>
        /// Gets the current page.
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// Gets the end index.
        /// </summary>
        public int EndIndex { get; private set; }

        /// <summary>
        /// Gets the end page.
        /// </summary>
        public int EndPage { get; private set; }

        /// <summary>
        /// Gets the pages.
        /// </summary>
        public IEnumerable<int> Pages { get; private set; }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets the start index.
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Gets the start page.
        /// </summary>
        public int StartPage { get; private set; }

        /// <summary>
        /// Gets the total items.
        /// </summary>
        public int TotalItems { get; private set; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// The calculate pages.
        /// </summary>
        /// <param name="totalItems">
        /// The total items.
        /// </param>
        /// <param name="currentPage">
        /// The current page.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="maxPages">
        /// The max pages.
        /// </param>
        /// <returns>
        /// The <see cref="Paginator"/>.
        /// </returns>
        private Paginator CalculatePages(int totalItems, int currentPage = 1, int pageSize = 10, int maxPages = 10)
        {
            // Calculate Total Pages
            var totalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize);

            // Ensure Current Page isn't out of range
            if (currentPage < 1) currentPage = 1;
            else if (currentPage > totalPages) currentPage = totalPages;

            int startPage, endPage;
            if (totalPages <= maxPages){
                // Total Pages less than max so show all pages
                startPage = 1;
                endPage = totalPages;
            }
            else{
                // Total Pages more than max so calculate start and end pages
                var maxPagesBeforeCurrentPage = (int)Math.Floor(maxPages / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling(maxPages / (decimal)2) - 1;
                if (currentPage <= maxPagesBeforeCurrentPage){
                    // Current Page near the start
                    startPage = 1;
                    endPage = maxPages;
                }
                else if (currentPage + maxPagesAfterCurrentPage >= totalPages){
                    // Current Page near the end
                    startPage = totalPages - maxPages + 1;
                    endPage = totalPages;
                }
                else{
                    // Current Page somewhere in the middle
                    startPage = currentPage - maxPagesBeforeCurrentPage;
                    endPage = currentPage + maxPagesAfterCurrentPage;
                }
            }

            // Calculate start and end item indexes
            var startIndex = (currentPage - 1) * pageSize;
            var endIndex = Math.Min(startIndex + pageSize - 1, totalItems - 1);

            // Create an array of pages that can be looped over
            var pages = Enumerable.Range(startPage, endPage + 1 - startPage);

            // Update object instance with all pager properties required by the view
            this.TotalItems = totalItems;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalPages = totalPages;
            this.StartPage = startPage;
            this.EndPage = endPage;
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;
            this.Pages = pages;

            return this;
        }
    }
}