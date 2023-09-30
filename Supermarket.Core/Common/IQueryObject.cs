using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Core.Common.Paging;

namespace Supermarket.Core.Common
{
    /// <summary>
    /// Marker for query objects.
    /// Represents some arguments for ordering, filtering, paging the source of data
    /// </summary>
    public interface IQueryObject
    {
        RecordsRange RecordsRange { get; }
    }
}
