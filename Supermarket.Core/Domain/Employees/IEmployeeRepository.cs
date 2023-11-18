using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Common.Paging;
using Supermarket.Core.UseCases.Admin;
using Supermarket.Core.UseCases.ManagerMenu;

namespace Supermarket.Core.Domain.Employees
{
    public interface IEmployeeRepository : ICrudRepository<Employee, int>
    {
        Task<EmployeeRole?> GetRoleByLoginAsync(string login);
        Task<EmployeeRole?> GetRoleByIdAsync(int employeeId);
        Task AddAsync(EmployeeRole employeeRole);
        Task UpdateAsync(EmployeeRole employeeRole);
        Task<ManagerMenuEmployeeDetail?> GetEmployeeDetail(int employeeId);
        Task<PagedResult<PossibleManagerForEmployee>> GetPossibleManagersForManager(int managerId, RecordsRange recordsRange);
        Task<PagedResult<PossibleManagerForEmployee>> GetPossibleManagersForAdmin(int supermarketId, RecordsRange recordsRange);
        Task<PagedResult<ManagerMenuEmployee>> GetSupermarketEmployeesForManager(int managerId, RecordsRange recordsRange);
        Task<PagedResult<ManagerMenuEmployee>> GetSupermarketEmployeesForAdmin(int supermarketId, RecordsRange recordsRange);
        Task<PagedResult<AdminEmployee>> GetAdminMenuEmployees(RecordsRange recordsRange);
        Task<AdminEmployeeDetail?> GetAdminEmployeeDetail(int employeeId);
    }
}
