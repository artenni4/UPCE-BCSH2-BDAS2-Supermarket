using Supermarket.Core.Common;
using Supermarket.Core.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.Employees
{
    public interface IEmployeeRepository : ICrudRepository<Employee, int, PagingQueryObject>
    {
        Task<Employee?> GetByLoginAsync(string login);
    }
}
