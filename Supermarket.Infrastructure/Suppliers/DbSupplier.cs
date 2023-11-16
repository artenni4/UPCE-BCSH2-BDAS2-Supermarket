using Dapper;
using Supermarket.Core.Domain.Suppliers;

namespace Supermarket.Infrastructure.Suppliers
{
    internal class DbSupplier : IDbEntity<Supplier, int, DbSupplier>
    {
        public required int dodavatel_id { get; init; }
        public required string nazev { get; init; }

        public static string TableName => "DODAVATELE";

        public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
        {
            nameof(dodavatel_id)
        };

        public Supplier ToDomainEntity() => new()
        {
            Id = dodavatel_id,
            Name = nazev
        };

        public static DbSupplier ToDbEntity(Supplier entity) => new()
        {
            dodavatel_id = entity.Id,
            nazev = entity.Name
        };

        public static DynamicParameters GetEntityIdParameters(int id) =>
            new DynamicParameters().AddParameter(nameof(dodavatel_id), id);

        public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();
    }
}
