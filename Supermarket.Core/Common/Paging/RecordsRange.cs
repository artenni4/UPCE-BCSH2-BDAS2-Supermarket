using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Common.Paging
{
    /// <summary>
    /// Specifies paging constraints for data source
    /// </summary>
    public class RecordsRange
    {
        /// <summary>
        /// Maximum records per one page
        /// </summary>
        public const int MaxPageSize = 200;

        /// <summary>
        /// Size of the page
        /// </summary>
        public required int PageSize { get; set; }

        /// <summary>
        /// Number of the page, starting from 1
        /// </summary>
        public required int PageNumber { get; set; }
    }
}
