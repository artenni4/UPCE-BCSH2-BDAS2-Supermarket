using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.Sales
{
    public interface ISaleRepository : ICrudRepository<Sale, int>
    {
        Task<int> AddAndGetIdAsync(Sale sale);
    }
}
