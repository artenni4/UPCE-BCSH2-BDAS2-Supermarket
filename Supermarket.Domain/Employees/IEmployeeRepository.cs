using Supermarket.Domain.Common;

namespace Supermarket.Domain.Employees
{
    public interface IEmployeeRepository : ICrudRepository<Employee, int>
    {
        Task<Employee?> GetByLoginAsync(string login);
    }
}
