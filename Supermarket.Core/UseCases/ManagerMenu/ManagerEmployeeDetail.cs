﻿using Supermarket.Core.Domain.Common;
using Supermarket.Core.Domain.Employees.Roles;

namespace Supermarket.Core.UseCases.ManagerMenu
{
    public class ManagerMenuEmployeeDetail : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required string Login { get; init; }
        public required DateTime HireDate { get; init; }
        public required bool IsCashier { get; init; }
        public required bool IsGoodsKeeper { get; init; }
        public required bool IsManager { get; init; }
        public required bool IsAdmin { get; init; }
    }
}
