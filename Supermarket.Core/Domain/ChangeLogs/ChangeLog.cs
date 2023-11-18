namespace Supermarket.Core.Domain.ChangeLogs;

public class ChangeLog
{
    public required string TableName { get; init; }
    public required string Operation { get; init; }
    public required string UserName { get; init; }
    public required DateTimeOffset TimeOccured { get; init; }
}