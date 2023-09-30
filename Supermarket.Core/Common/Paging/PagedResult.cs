using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Common.Paging
{
    /// <summary>
    /// Represents one page of results returned by a query
    /// </summary>
    /// <typeparam name="TItem">type of items in the result</typeparam>
    /// <param name="Items">list of items</param>
    /// <param name="PageNumber">current page number, counting from 1</param>
    /// <param name="TotalItems">total items in the source</param>
    public record PagedResult<TItem>(IReadOnlyList<TItem> Items, int PageNumber, int TotalItems)
    {
        /// <summary>
        /// Whether next page contains items
        /// </summary>
        public bool HasNext => PageNumber * Items.Count < TotalItems;

        /// <summary>
        /// Whether previous page contains items
        /// </summary>
        public bool HasPrevious => PageNumber == 1;

        /// <summary>
        /// Total count of pages in the source
        /// </summary>
        public int PageCount => (int)Math.Ceiling(TotalItems / (double)Items.Count);
    }
}
