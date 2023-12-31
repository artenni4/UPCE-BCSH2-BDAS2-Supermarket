﻿using Supermarket.Core.Domain.Common;

namespace Supermarket.Core.Domain.CashBoxes;

public class CashBox : IEntity<int>
{
    public required int Id { get; init; }
    public required int SupermarketId { get; init; }
    public required string Name { get; init; }
    public required string Code { get; init; }
    public required string? Notes { get; init; }
}