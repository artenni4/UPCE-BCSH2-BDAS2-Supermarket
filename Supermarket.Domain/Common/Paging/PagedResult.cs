namespace Supermarket.Domain.Common.Paging
{
    /// <summary>
    /// Represents one page of results returned by a query
    /// </summary>
    /// <typeparam name="TItem">type of items in the result</typeparam>
    /// <param name="Items">list of items</param>
    /// <param name="PageNumber">current page number, counting from 1</param>
    /// <param name="TotalItems">total items in the source</param>
    public record PagedResult<TItem>(IReadOnlyList<TItem> Items, int PageNumber, int PageSize, int TotalItems)
    {
        public PagedResult(IReadOnlyList<TItem> Items, RecordsRange recordsRange, int TotalItems) 
            : this(Items, recordsRange.PageNumber, recordsRange.PageSize, TotalItems)
        {
        }

        public static PagedResult<TItem> Empty() => new(Array.Empty<TItem>(), 1, 0, 0);

        /// <summary>
        /// Whether next page contains items
        /// </summary>
        public bool HasNext => PageNumber * PageSize < TotalItems;

        /// <summary>
        /// Whether previous page contains items
        /// </summary>
        public bool HasPrevious => PageNumber > 1;

        /// <summary>
        /// Total count of pages in the source
        /// </summary>
        public int PageCount => (int)Math.Ceiling(TotalItems / (double)PageSize);
    }
}
