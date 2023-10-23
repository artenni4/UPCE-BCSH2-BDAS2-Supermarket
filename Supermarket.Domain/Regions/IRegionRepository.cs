using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.Regions;

public interface IRegionRepository : ICrudRepository<Region, int, PagingQueryObject>
{
    
}