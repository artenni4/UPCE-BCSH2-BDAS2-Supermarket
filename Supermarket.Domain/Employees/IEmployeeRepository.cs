using Supermarket.Domain.Common;
using Supermarket.Domain.Common.Paging;

namespace Supermarket.Domain.Employees
{
    public interface IEmployeeRepository : ICrudRepository<Employee, int, PagingQueryObject>
    {
        Task<Employee?> GetByLoginAsync(string login);
    }
}
