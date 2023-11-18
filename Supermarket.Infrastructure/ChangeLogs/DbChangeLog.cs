using Supermarket.Core.Domain.ChangeLogs;

namespace Supermarket.Infrastructure.ChangeLogs;

public class DbChangeLog
{
    public required string tabulka { get; init; }
    public required string operace { get; init; }
    public required string uzivatel { get; init; }
    public required DateTimeOffset cas { get; init; }

    public ChangeLog ToDomainEntity() => new ChangeLog()
    {
        TableName = tabulka,
        Operation = operace,
        UserName = uzivatel,
        TimeOccured = cas
    };
}