using Oracle.ManagedDataAccess.Client;
using Supermarket.Infrastructure.Common;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Supermarket.Domain.Common.Paging;
using Supermarket.Domain.Employees;

namespace Supermarket.Infrastructure.Employees
{
    internal class EmployeeRepository : CrudRepositoryBase<Employee, int, EmployeeRepository.DbEmployee, PagingQueryObject>, IEmployeeRepository
    {
        public EmployeeRepository(OracleConnection oracleConnection) : base(oracleConnection)
        {
        }

        public class DbEmployee
        {
            public required int zamestnanec_id { get; init; }
        }

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
                Roles = new[] { "CASHIER" },
                SupermarketId = 1,
            });
        }

        protected override string TableName => "ZAMESTNANCI";
        protected override IReadOnlyList<string> IdentityColumns { get; } = new[] { nameof(DbEmployee.zamestnanec_id) };
        protected override Employee MapToEntity(DbEmployee dbEntity)
        {
            throw new NotImplementedException();
        }

        protected override DbEmployee MapToDbEntity(Employee entity)
        {
            throw new NotImplementedException();
        }

        protected override DynamicParameters GetIdentityValues(int id)
        {
            throw new NotImplementedException();
        }

        protected override int ExtractIdentity(DynamicParameters dynamicParameters)
        {
            throw new NotImplementedException();
        }

        protected override string? GetCustomPagingCondition(PagingQueryObject queryObject, out DynamicParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
