﻿using Dapper;
using Supermarket.Core.Domain.Employees;

namespace Supermarket.Infrastructure.Employees;

internal class DbEmployee : IDbEntity<Employee, int, DbEmployee>
{
    public required int zamestnanec_id { get; init; }
    public required string login { get; init; }
    public required byte[] heslo_hash { get; init; }
    public required byte[] heslo_salt { get; init; }
    public required string jmeno { get; init; }
    public required string prijmeni { get; init; }
    public required DateTimeOffset datum_nastupu { get; init; }
    public required int? supermarket_id { get; init; }
    public required int? manazer_id { get; init; }
    public required string? rodne_cislo { get; init; }

    public static string TableName => "ZAMESTNANCI";
    public static IReadOnlySet<string> IdentityColumns { get; } = new HashSet<string>
    {
        nameof(zamestnanec_id)
    };

    public Employee ToDomainEntity() => new()
    {
        Id = zamestnanec_id,
        Login = login,
        PasswordHash = heslo_hash,
        PasswordHashSalt = heslo_salt,
        Name = jmeno,
        Surname = prijmeni,
        StartedWorking = datum_nastupu,
        ManagerId = manazer_id,
        SupermarketId = supermarket_id,
        PersonalNumber = rodne_cislo
    };

    public static DbEmployee ToDbEntity(Employee entity) => new()
    {
        zamestnanec_id = entity.Id,
        login = entity.Login,
        heslo_hash = entity.PasswordHash,
        heslo_salt = entity.PasswordHashSalt,
        jmeno = entity.Name,
        prijmeni = entity.Surname,
        datum_nastupu = entity.StartedWorking,
        manazer_id = entity.ManagerId,
        supermarket_id = entity.SupermarketId,
        rodne_cislo = entity.PersonalNumber
    };

    public static DynamicParameters GetEntityIdParameters(int id) =>
        new DynamicParameters().AddParameter(nameof(zamestnanec_id), id);

    public DynamicParameters GetInsertingValues() => this.GetPropertiesExceptIdentity();
}