using Supermarket.Core.Common;

namespace Supermarket.Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
