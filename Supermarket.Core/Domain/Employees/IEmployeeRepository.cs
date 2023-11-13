using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Core.Domain.Employees
{
    public interface IEmployeeRepository : ICrudRepository<Employee, int>
    {
        Task<Employee?> GetByLoginAsync(string login);
        Task<PagedResult<ManagerMenuEmployee>> GetSupermarketEmployees(int supermarketId, RecordsRange recordsRange);
    }
}
