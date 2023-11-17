using Supermarket.Core.UseCases.Admin;

namespace Supermarket.Infrastructure.Employees
{
    internal class DbAdminMenuEmployee
    {
        public required int zamestnanec_id { get; init; }
        public required string jmeno { get; init; }
        public required string prijmeni { get; init; }
        public required DateTime datum_nastupu { get; init; }
        public required int? supermarket_id { get; init; }
        public required string supermarket_nazev { get; init; }
        public required string role { get; init; }
        public required string rodne_cislo { get; init; }

        public static string TableName => "ZAMESTNANCI";
        public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
        {
            nameof(zamestnanec_id)
        };

        public AdminEmployee ToDomainEntity() => new()
        {
            Id = zamestnanec_id,
            Name = jmeno,
            Surname = prijmeni,
            HireDate = datum_nastupu,
            SupermarketId = supermarket_id,
            SupermarketName = supermarket_nazev,
            Roles = role,
            PersonalNumber = rodne_cislo
        };

    }
}
