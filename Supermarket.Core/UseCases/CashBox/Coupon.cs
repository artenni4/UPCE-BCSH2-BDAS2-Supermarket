namespace Supermarket.Core.UseCases.CashBox;

public class Coupon : IEquatable<Coupon>
{
    public required string Code { get; init; }
    public required decimal Discount { get; init; }

    public bool Equals(Coupon? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Code == other.Code;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Coupon)obj);
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }
}