using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.Employees
{
    public interface IEmployeeRepository : ICrudRepository<Employee, int>
    {
        Task<Employee?> GetByLoginAsync(string login);
    }
}
