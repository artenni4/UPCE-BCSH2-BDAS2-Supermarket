using Supermarket.Domain.Common;

namespace Supermarket.Domain.SoldProducts
{
    public interface ISoldProductRepository : ICrudRepository<SoldProduct, int>
    {
    }
}
