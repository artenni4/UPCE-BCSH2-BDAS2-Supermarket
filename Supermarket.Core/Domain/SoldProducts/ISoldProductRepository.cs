using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.SoldProducts;

public interface ISoldProductRepository : ICrudRepository<SoldProduct, SoldProductId>
{
}