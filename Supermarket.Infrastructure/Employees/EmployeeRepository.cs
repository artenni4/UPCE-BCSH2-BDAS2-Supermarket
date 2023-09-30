using Supermarket.Core.Common.Paging;
using Supermarket.Core.Employees;
using Supermarket.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Infrastructure.Employees
{
    public class EmployeeRepository : CrudRepositoryBase<Employee, int, PagingQueryObject>, IEmployeeRepository
    {
        public override Task<PagedResult<Employee>> GetPagedAsync(PagingQueryObject queryObject) => GetRecordsRangeAsync(queryObject.RecordsRange);

        public Task<Employee?> GetByLoginAsync(string login)
        {
            if (login != "BIBA") return Task.FromResult((Employee?)null);

            var passwordBytes = Encoding.UTF8.GetBytes("ILOVE");
            var salt = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
            Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Array.Copy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

            byte[] hashBytes = SHA256.HashData(combinedBytes);

            return Task.FromResult((Employee?)new Employee
            {
                Id = 1,
                Login = "BIBA",
                Name = "David",
                Surname = "Biba",
                StartedWorking = new DateTimeOffset(2020, 10, 1, 0, 0, 0, TimeSpan.Zero),
                PasswordHash = hashBytes,
                PasswordHashSalt = salt,
                Roles = new[] { EmployeeRole.Cashier },
            });
        }
    }
}
