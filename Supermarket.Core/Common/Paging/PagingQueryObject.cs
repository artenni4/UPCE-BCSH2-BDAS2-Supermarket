using Supermarket.Core.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Common.Paging
{   
    /// <summary>
    /// Query object that specifies only records range
    /// </summary>
    public class PagingQueryObject : IQueryObject
    {
        public required RecordsRange RecordsRange { get; init; }
    }
}
