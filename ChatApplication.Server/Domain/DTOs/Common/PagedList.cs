namespace ChatApplication.Server.Domain.DTOs.Common
{
    /// <summary>
    /// Represents a paged list of items with pagination metadata.
    /// </summary>
    /// <typeparam name="T">The type of items in the paged list.</typeparam>
    public class PagedList<T>
    {
        /// <summary>
        /// The items on the current page.
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// The current page number (starting from 1).
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The total number of items across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The total number of pages based on the total count and page size.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Indicates whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Indicates whether there is a next page.
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="items">The items on the current page.</param>
        /// <param name="totalCount">The total number of items across all pages.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PagedList()
        {
        }
    }

}
