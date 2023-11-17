using Supermarket.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Core.UseCases.Admin
{
    public class AdminEmployee : IEntity<int>
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required DateTime HireDate { get; init; }
        public required string Roles { get; init; }
        public required int? SupermarketId { get; init; }
        public required string? SupermarketName { get; init; }
        public required string PersonalNumber { get; init; }
    }
}
