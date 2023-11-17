﻿using Supermarket.Core.Domain.Auth.LoggedEmployees;

namespace Supermarket.Core.UseCases.Admin
{
    public class AdminEmployeeData
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required string Login { get; init; }
        public required DateTime HireDate { get; init; }
        public required int? ManagerId { get; init; }
        public required int? SupermarketId { get; init; }
        public required IReadOnlySet<SupermarketEmployeeRole> Roles { get; init; }
    }
}
